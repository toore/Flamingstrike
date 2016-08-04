using System.Collections.Generic;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Extensions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.Tests.GameEngine;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameboardViewModelTests
    {
        private readonly IGame _game;
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IRegions _regions;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly GameboardViewModelFactory _gameboardViewModelFactory;

        public GameboardViewModelTests()
        {
            _game = Substitute.For<IGame>();
            _interactionStateFsm = Substitute.For<IInteractionStateFsm>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _regions = Substitute.For<IRegions>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();

            ResourceManager.Instance = Substitute.For<IResourceManager>();

            _gameboardViewModelFactory = new GameboardViewModelFactory(
                _interactionStateFsm,
                _interactionStateFactory,
                _regions,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }

        [Fact]
        public void Activate_initializes_WorldMapViewModel()
        {
            var region = Substitute.For<IRegion>();
            var anotherRegion = Substitute.For<IRegion>();
            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            var worldMapViewModel = new WorldMapViewModel();
            _regions.GetAll().Returns(new[]
            {
                region,
                anotherRegion
            });
            _game.GetTerritory(region).Returns(territory);
            _game.GetTerritory(anotherRegion).Returns(anotherTerritory);
            var sut = Initialize(activate: false);
            _worldMapViewModelFactory.Create(
                Argx.IsEquivalentReadOnly(territory, anotherTerritory),
                sut.OnRegionClick,
                Arg.Is<IEnumerable<IRegion>>(x => x.IsEmpty()))
                .Returns(worldMapViewModel);

            sut.Activate();

            sut.WorldMapViewModel.Should().Be(worldMapViewModel);
        }

        [Fact]
        public void Player_takes_turn()
        {
            var currentPlayer = new Player("player taking turn");
            _game.CurrentPlayer.Returns(currentPlayer);

            var sut = Initialize();

            sut.PlayerName.Should().Be("player taking turn");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Can_end_turn(bool canEndTurn)
        {
            _game.CanEndTurn().Returns(canEndTurn);

            var sut = Initialize();

            sut.CanEndTurn().Should().Be(canEndTurn);
        }

        [Fact]
        public void When_turn_ends_player_is_updated()
        {
            var sut = Initialize();
            var nextPlayer = new Player("next player");
            _game.CurrentPlayer.Returns(nextPlayer);

            sut.EndTurn();

            sut.PlayerName.Should().Be("next player");
        }

        [Fact]
        public void When_turn_ends_the_interaction_state_defaults_to_draft_armies()
        {
            var draftArmiesInteractionStateForFirstPlayer = Substitute.For<IInteractionState>();
            var draftArmiesInteractionStateForNextPlayer = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateDraftArmiesInteractionState(_game)
                .Returns(
                    draftArmiesInteractionStateForFirstPlayer,
                    draftArmiesInteractionStateForNextPlayer);
            var sut = Initialize();

            sut.EndTurn();

            _interactionStateFsm.Received().Set(draftArmiesInteractionStateForNextPlayer);
        }

        [Fact]
        public void Show_game_over_when_game_is_updated_after_user_action()
        {
            _game.CurrentPlayer.Returns(new Player("the winner"));
            var gameOverViewModel = new GameOverViewModel("");
            _gameOverViewModelFactory.Create("the winner").Returns(gameOverViewModel);
            _game.IsGameOver().Returns(true);
            var sut = Initialize();

            sut.OnRegionClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            var sut = Initialize();
            _game.IsGameOver().Returns(false);

            sut.OnRegionClick(null);

            _windowManager.DidNotReceiveWithAnyArgs().ShowDialog(null);
        }

        [Fact]
        public void End_game_shows_confirm_dialog()
        {
            var sut = Initialize();

            sut.EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
        }

        [Fact]
        public void Clicked_territory_is_routed_to_state_fsm()
        {
            var region = Substitute.For<IRegion>();
            var sut = Initialize();

            sut.OnRegionClick(region);

            _interactionStateFsm.Received().OnClick(region);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Can_activate_fortify(bool canFreeMove)
        {
            _game.CanFreeMove().Returns(canFreeMove);

            var sut = Initialize();

            sut.CanActivateFortify().Should().Be(canFreeMove);
        }

        [Fact]
        public void Activates_fortify()
        {
            var fortifyState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifySelectInteractionState(_game).Returns(fortifyState);
            var sut = Initialize();

            sut.EnterFortifyMode();

            _interactionStateFsm.Received().Set(fortifyState);
        }

        private GameboardViewModel Initialize(bool activate = true)
        {
            var sut = (GameboardViewModel)_gameboardViewModelFactory.Create(_game);
            if (activate)
            {
                sut.Activate();
            }
            return sut;
        }
    }
}