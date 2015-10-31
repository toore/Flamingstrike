using System;
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

            LanguageResources.Instance = Substitute.For<ILanguageResources>();

            var viewModel = Substitute.For<ITerritoryLayoutViewModel>();
            var layoutViewModel1 = viewModel;
            var viewModel2 = Substitute.For<IWorldMapItemViewModel>();
            var textViewModel1 = viewModel2;
            var viewModel1 = Substitute.For<ITerritoryLayoutViewModel>();
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
        public void Initializes_WorldMapViewModel_when_activated()
        {
            var sut = Initialize(activate: false);
            _worldMapViewModelFactory.Create(_game.Territories, sut.OnTerritoryClick, Arg.Is<IEnumerable<ITerritoryId>>(x => x.IsEmpty()))
                .Returns(_worldMapViewModel);

            sut.Activate();

            sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Player_takes_turn()
        {
            var player = Substitute.For<IPlayer>();
            player.PlayerId.Name.Returns("player");
            _game.CurrentPlayer.Returns(player);

            var sut = Initialize();

            sut.PlayerName.Should().Be("player");
        }

        [Fact]
        public void Player_is_updated_when_turn_ends()
        {
            var sut = Initialize();
            var player = Substitute.For<IPlayer>();
            player.PlayerId.Name.Returns("next player");
            _game.CurrentPlayer.Returns(player);

            sut.EndTurn();

            sut.PlayerName.Should().Be("next player");
        }

        [Fact]
        public void Show_game_over_when_game_is_updated()
        {
            var winner = Substitute.For<IPlayer>();
            winner.PlayerId.Name.Returns("winner");
            _game.CurrentPlayer.Returns(winner);
            var sut = Initialize();
            var gameOverViewModel = new GameOverViewModel("");
            _gameOverViewModelFactory.Create("winner").Returns(gameOverViewModel);
            _game.IsGameOver().Returns(true);

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
        public void Interaction_state_receives_on_click_when_location_is_clicked()
        {
            var stateController = Substitute.For<IStateController>();
            _stateControllerFactory.Create(_game).Returns(stateController);
            var territoryId = Substitute.For<ITerritoryId>();

            var sut = Initialize();
            sut.OnTerritoryClick(territoryId);

            stateController.Received().OnClick(territoryId);
        }

        [Fact]
        public void Fortifies_armies()
        {
            var sut = Initialize();
            sut.Fortify();

            //_interactionState.Received().Fortify();
            throw new NotImplementedException();
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