using System.Collections.Generic;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Extensions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameboardViewModelTests
    {
        private readonly IGame _game;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMap _worldMap;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly GameboardViewModelFactory _gameboardViewModelFactory;

        public GameboardViewModelTests()
        {
            _game = Substitute.For<IGame>();
            _stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _worldMap = Substitute.For<IWorldMap>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();

            ResourceManager.Instance = Substitute.For<IResourceManager>();

            var viewModel = Substitute.For<IRegionViewModel>();
            var layoutViewModel1 = viewModel;
            var viewModel2 = Substitute.For<IWorldMapItemViewModel>();
            var textViewModel1 = viewModel2;
            var viewModel1 = Substitute.For<IRegionViewModel>();
            var layoutViewModel2 = viewModel1;
            var viewModel3 = Substitute.For<IWorldMapItemViewModel>();
            var textViewModel2 = viewModel3;

            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel2);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel2);

            _gameboardViewModelFactory = new GameboardViewModelFactory(
                _stateControllerFactory,
                _interactionStateFactory,
                _worldMap,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }

        [Fact]
        public void Activate_initializes_WorldMapViewModel()
        {
            var sut = Initialize(activate: false);
            _worldMapViewModelFactory.Create(_game.GetTerritories(), sut.OnTerritoryClick, Arg.Is<IEnumerable<IRegion>>(x => x.IsEmpty()))
                .Returns(_worldMapViewModel);

            sut.Activate();

            sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Player_takes_turn()
        {
            var player = Substitute.For<IPlayer>();
            player.Name.Returns("player");
            _game.CurrentPlayer.Returns(player);

            var sut = Initialize();

            sut.PlayerName.Should().Be("player");
        }

        [Fact]
        public void Player_is_updated_when_turn_ends()
        {
            var sut = Initialize();
            var player = Substitute.For<IPlayer>();
            player.Name.Returns("next player");
            _game.CurrentPlayer.Returns(player);

            sut.EndTurn();

            sut.PlayerName.Should().Be("next player");
        }

        [Fact]
        public void Show_game_over_when_game_is_updated_after_user_action()
        {
            var winner = Substitute.For<IPlayer>();
            winner.Name.Returns("the winner's name");
            _game.CurrentPlayer.Returns(winner);
            var gameOverViewModel = new GameOverViewModel("");
            _gameOverViewModelFactory.Create("the winner's name").Returns(gameOverViewModel);
            _game.IsGameOver().Returns(true);
            var sut = Initialize();

            sut.OnTerritoryClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            var sut = Initialize();
            _game.IsGameOver().Returns(false);

            sut.OnTerritoryClick(null);

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
        public void Clicked_territory_is_proxied_to_state_controller()
        {
            var stateController = Substitute.For<IStateController>();
            _stateControllerFactory.Create(_game).Returns(stateController);
            var territoryId = Substitute.For<IRegion>();
            var sut = Initialize();

            sut.OnTerritoryClick(territoryId);

            stateController.Received().OnClick(territoryId);
        }

        [Fact]
        public void Fortifies_armies()
        {
            var stateController = Substitute.For<IStateController>();
            _stateControllerFactory.Create(_game).Returns(stateController);
            var fortifyState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifySelectState().Returns(fortifyState);
            var sut = Initialize();

            sut.Fortify();

            stateController.CurrentState.Should().Be(fortifyState);
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