using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEventAggregator _gameEventAggregator;
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly GameboardViewModel _sut;

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
            _gameEventAggregator = Substitute.For<IEventAggregator>();

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

            _sut = new GameboardViewModel(
                _game,
                _stateControllerFactory,
                _interactionStateFactory,
                _worldMap,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _gameEventAggregator);
        }

        [Fact]
        public void Initializes_WorldMapViewModel_when_activated()
        {
            _worldMapViewModelFactory.Create(_game.Territories, _sut.OnTerritoryClick, Arg.Is<IEnumerable<ITerritoryId>>(x => x.IsEmpty()))
                .Returns(_worldMapViewModel);

            _sut.Activate();

            _sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Player_takes_turn()
        {
            var player = Substitute.For<IPlayer>();
            player.PlayerId.Name.Returns("player");
            _game.CurrentPlayer.Returns(player);

            _sut.Activate();

            _sut.PlayerName.Should().Be("player");
        }

        [Fact]
        public void Player_is_updated_when_turn_ends()
        {
            _sut.Activate();
            var player = Substitute.For<IPlayer>();
            player.PlayerId.Name.Returns("next player");
            _game.CurrentPlayer.Returns(player);

            _sut.EndTurn();

            _sut.PlayerName.Should().Be("next player");
        }

        [Fact]
        public void Show_game_over_when_game_is_updated()
        {
            var winner = Substitute.For<IPlayer>();
            winner.PlayerId.Name.Returns("winner");
            _game.CurrentPlayer.Returns(winner);
            _sut.Activate();
            var gameOverViewModel = new GameOverViewModel("");
            _gameOverViewModelFactory.Create("winner").Returns(gameOverViewModel);
            _game.IsGameOver().Returns(true);

            _sut.OnTerritoryClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            _sut.Activate();
            _game.IsGameOver().Returns(false);

            _sut.OnTerritoryClick(null);

            _windowManager.DidNotReceiveWithAnyArgs().ShowDialog(null);
        }

        [Fact]
        public void End_game_shows_confirm_dialog()
        {
            _sut.Activate();
            _sut.EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
        }

        [Fact]
        public void Interaction_state_receives_on_click_when_location_is_clicked()
        {
            var stateController = Substitute.For<IStateController>();
            _stateControllerFactory.Create(_game).Returns(stateController);
            var territoryId = Substitute.For<ITerritoryId>();

            _sut.Activate();
            _sut.OnTerritoryClick(territoryId);

            stateController.Received().OnClick(territoryId);
        }

        [Fact]
        public void Fortifies_armies()
        {
            _sut.Fortify();

            //_interactionState.Received().Fortify();
            throw new NotImplementedException();
        }
    }
}