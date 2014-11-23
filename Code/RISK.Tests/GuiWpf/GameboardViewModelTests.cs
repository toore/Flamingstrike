using System;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameboardViewModelTests
    {
        private readonly IGame _game;
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly ITerritory _territory1;
        private readonly ITerritoryViewModelColorInitializer _territoryViewModelColorInitializer;
        private readonly IInteractionState _initialInteractionState;
        private readonly IInteractionState _nextInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _nextPlayer;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _gameEventAggregator;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;

        public GameboardViewModelTests()
        {
            _game = Substitute.For<IGame>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _territoryViewModelColorInitializer = Substitute.For<ITerritoryViewModelColorInitializer>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            LanguageResources.Instance = Substitute.For<ILanguageResources>();

            _territory1 = Substitute.For<ITerritory>();

            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            _initialInteractionState = Substitute.For<IInteractionState>();
            _initialInteractionState.Player.Returns(_currentPlayer);
            _nextInteractionState = Substitute.For<IInteractionState>();
            _nextInteractionState.Player.Returns(_nextPlayer);

            var viewModel = Substitute.For<ITerritoryLayoutViewModel>();
            var layoutViewModel1 = viewModel;
            var viewModel2 = Substitute.For<ITerritoryTextViewModel>();
            var textViewModel1 = viewModel2;
            var viewModel1 = Substitute.For<ITerritoryLayoutViewModel>();
            var layoutViewModel2 = viewModel1;
            var viewModel3 = Substitute.For<ITerritoryTextViewModel>();
            var textViewModel2 = viewModel3;

            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel2);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel2);

            var worldMap = Substitute.For<IWorldMap>();
            _game.WorldMap.Returns(worldMap);

            _worldMapViewModelFactory.Create(Arg.Is(worldMap), Arg.Any<Action<ITerritory>>()).Returns(_worldMapViewModel);
        }

        private GameboardViewModel Create()
        {
            return new GameboardViewModel(
                _game, 
                _worldMapViewModelFactory, 
                _territoryViewModelColorInitializer, 
                _windowManager,
                _gameOverViewModelFactory, 
                _dialogManager, 
                _gameEventAggregator);
        }

        [Fact]
        public void Initializes_WorldMapViewModel()
        {
            Create().WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Player1_takes_first_turn()
        {
            _game.CurrentInteractionState.Returns(_initialInteractionState);

            AssertCurrentPlayer(_currentPlayer);
        }

        [Fact]
        public void Player2_takes_second_turn()
        {
            _game.CurrentInteractionState.Returns(_initialInteractionState, _nextInteractionState);

            Create().EndTurn();

            AssertCurrentPlayer(_nextPlayer);
        }

        private void AssertCurrentPlayer(IPlayer expected)
        {
            Create().Player.Should().Be(expected);
        }

        [Fact]
        public void Ends_turn_and_gets_next_turn()
        {
            _game.CurrentInteractionState.Returns(_initialInteractionState, _nextInteractionState);

            var sut = Create();
            _game.ClearReceivedCalls();

            sut.EndTurn();

            //_initialInteractionState.Received(1).EndTurn();
            //_game.Received(1).GetNextTurn();
            sut.Player.Should().Be(_nextPlayer);
        }

        [Fact]
        public void When_winning_game_over_dialog_should_be_shown()
        {
            _game.CurrentInteractionState.Returns(_initialInteractionState);

            _game.IsGameOver().Returns(true);
            var gameOverViewModel = new GameOverViewModel(_currentPlayer);
            _gameOverViewModelFactory.Create(_currentPlayer).Returns(gameOverViewModel);

            Create().OnLocationClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            _game.IsGameOver().Returns(false);
            var gameOverViewModel = new GameOverViewModel(_currentPlayer);

            Create().OnLocationClick(null);

            _windowManager.DidNotReceiveWithAnyArgs().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void End_game_shows_confirm_dialog()
        {
            Create().EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
        }

        [Fact]
        public void Interaction_state_receives_on_click_when_location_is_clicked()
        {
            _game.CurrentInteractionState.Returns(_initialInteractionState);

            Create().OnLocationClick(_territory1);

            _initialInteractionState.Received().OnClick(_territory1);
        }
    }
}