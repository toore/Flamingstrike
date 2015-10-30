using System;
using Caliburn.Micro;
using FluentAssertions;
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
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly ITerritoryId _territory1;
        private readonly IInteractionState _firstPlayerInteractionState;
        private readonly IInteractionState _nextPlayerInteractionState;
        private readonly IPlayer _currentPlayerId;
        private readonly IPlayer _nextPlayerId;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _gameEventAggregator;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private GameboardViewModel _sut;
        private IStateControllerFactory _stateControllerFactory;
        private IInteractionStateFactory _interactionStateFactory;
        private IGame _game;
        private IInteractionState _interactionState;
        private IStateController _stateController;

        public GameboardViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            LanguageResources.Instance = Substitute.For<ILanguageResources>();

            _territory1 = Substitute.For<ITerritoryId>();

            _currentPlayerId = Substitute.For<IPlayer>();
            _nextPlayerId = Substitute.For<IPlayer>();

            _firstPlayerInteractionState = Substitute.For<IInteractionState>();
            //_firstPlayerInteractionState.Player.Returns(_currentPlayer);
            _nextPlayerInteractionState = Substitute.For<IInteractionState>();
            //_nextPlayerInteractionState.Player.Returns(_nextPlayer);

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

            var worldMap = Substitute.For<IWorldMap>();

            //_worldMapViewModelFactory.Create(Arg.Is(worldMap), Arg.Any<Action<ITerritory>>(), Arg.Any<IEnumerable<ITerritory>>()).Returns(_worldMapViewModel);

            _sut = new GameboardViewModel(
                _game,
                _stateControllerFactory,
                _interactionStateFactory,
                worldMap,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _gameEventAggregator);
        }

        [Fact]
        public void Initializes_WorldMapViewModel_upon_activation()
        {
            _sut.Activate();

            _sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void First_player_takes_first_turn()
        {
            _game.CurrentPlayer.Returns(_currentPlayerId);

            _sut.Activate();

            AssertCurrentPlayer(_currentPlayerId);
        }

        [Fact]
        public void Next_player_takes_second_turn()
        {
            _game.CurrentPlayer.Returns(_nextPlayerId);

            _sut.EndTurn();

            AssertCurrentPlayer(_nextPlayerId);
        }

        private void AssertCurrentPlayer(IPlayer expected)
        {
            _sut.PlayerId.Should().Be(expected);
        }

        [Fact]
        public void Ends_turn_and_gets_next_turn()
        {
            _game.CurrentPlayer.Returns(_nextPlayerId);

            var sut = _sut;
            sut.EndTurn();

            sut.PlayerId.Should().Be(_nextPlayerId);
        }

        [Fact]
        public void Show_game_over_when_game_is_updated()
        {
            _game.CurrentPlayer.Returns(_currentPlayerId);

            var gameOverViewModel = new GameOverViewModel(_currentPlayerId.PlayerId);
            _gameOverViewModelFactory.Create(_currentPlayerId.PlayerId).Returns(gameOverViewModel);

            _game.IsGameOver().Returns(true);

            _sut.OnTerritoryClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            _game.IsGameOver().Returns(false);
            var gameOverViewModel = new GameOverViewModel(_currentPlayerId.PlayerId);

            _sut.OnTerritoryClick(null);

            _windowManager.DidNotReceiveWithAnyArgs().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void End_game_shows_confirm_dialog()
        {
            _sut.EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
        }

        [Fact]
        public void Interaction_state_receives_on_click_when_location_is_clicked()
        {
            _sut.OnTerritoryClick(_territory1);

            _interactionState.Received().OnClick(_stateController, _territory1);
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