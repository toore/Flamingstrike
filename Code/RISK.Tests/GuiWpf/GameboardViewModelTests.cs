using System;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameboardViewModelTests
    {
        private readonly IGame _game;
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly ILocation _location1;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IInteractionState _initialInteractionState;
        private readonly IInteractionState _nextInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _nextPlayer;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _gameEventAggregator;
        private readonly ILocation[] _locations;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;

        public GameboardViewModelTests()
        {
            _game = Substitute.For<IGame>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            LanguageResources.Instance = Substitute.For<ILanguageResources>();

            _location1 = Substitute.For<ILocation>();
            var location2 = Substitute.For<ILocation>();
            _locations = new[]
            {
                _location1,
                location2
            };

            var worldMap = Substitute.For<IWorldMap>();
            var territory1 = new Territory(_location1);
            var territory2 = new Territory(location2);
            worldMap.GetTerritory(_location1).Returns(territory1);
            worldMap.GetTerritory(location2).Returns(territory2);
            _game.WorldMap.Returns(worldMap);

            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            _initialInteractionState = Substitute.For<IInteractionState>();
            _initialInteractionState.Player.Returns(_currentPlayer);
            _nextInteractionState = Substitute.For<IInteractionState>();
            _nextInteractionState.Player.Returns(_nextPlayer);

            var layoutViewModel1 = StubLayoutViewModel(_location1);
            var textViewModel1 = StubTextViewModel(_location1);
            var layoutViewModel2 = StubLayoutViewModel(location2);
            var textViewModel2 = StubTextViewModel(location2);

            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(layoutViewModel2);
            _worldMapViewModel.WorldMapViewModels.Add(textViewModel2);

            _worldMapViewModelFactory.Create(Arg.Is(worldMap), Arg.Any<Action<ILocation>>()).Returns(_worldMapViewModel);
        }

        private GameboardViewModel Create()
        {
            return new GameboardViewModel(
                _game, 
                _locations, 
                _worldMapViewModelFactory, 
                _territoryViewModelUpdater, 
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

            Create().OnLocationClick(_location1);

            _initialInteractionState.Received().OnClick(_location1);
        }


        private ITerritoryLayoutViewModel StubLayoutViewModel(ILocation location)
        {
            var viewModel = Substitute.For<ITerritoryLayoutViewModel>();
            viewModel.Location.Returns(location);
            return viewModel;
        }

        private ITerritoryTextViewModel StubTextViewModel(ILocation location)
        {
            var viewModel = Substitute.For<ITerritoryTextViewModel>();
            viewModel.Location.Returns(location);
            return viewModel;
        }
    }
}