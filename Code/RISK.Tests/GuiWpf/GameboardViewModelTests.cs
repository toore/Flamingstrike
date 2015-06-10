using System;
using System.Collections.Generic;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameboardViewModelTests
    {
        private readonly IGameAdapter _gameAdapter;
        private readonly WorldMapViewModel _worldMapViewModel;
        private readonly ITerritory _territory1;
        private readonly IInteractionState _firstPlayerInteractionState;
        private readonly IInteractionState _nextPlayerInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _nextPlayer;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _gameEventAggregator;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private GameboardViewModel _sut;

        public GameboardViewModelTests()
        {
            _gameAdapter = Substitute.For<IGameAdapter>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            LanguageResources.Instance = Substitute.For<ILanguageResources>();

            _territory1 = Substitute.For<ITerritory>();

            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            _firstPlayerInteractionState = Substitute.For<IInteractionState>();
            _firstPlayerInteractionState.Player.Returns(_currentPlayer);
            _nextPlayerInteractionState = Substitute.For<IInteractionState>();
            _nextPlayerInteractionState.Player.Returns(_nextPlayer);

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
            _gameAdapter.WorldMap.Returns(worldMap);

            _worldMapViewModelFactory.Create(Arg.Is(worldMap), Arg.Any<Action<ITerritory>>(), Arg.Any<IEnumerable<ITerritory>>()).Returns(_worldMapViewModel);

            _sut = new GameboardViewModel(
                _gameAdapter,
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
            _gameAdapter.Player.Returns(_currentPlayer);

            _sut.Activate();

            AssertCurrentPlayer(_currentPlayer);
        }

        [Fact]
        public void Next_player_takes_second_turn()
        {
            _gameAdapter.Player.Returns(_nextPlayer);

            _sut.EndTurn();

            AssertCurrentPlayer(_nextPlayer);
        }

        private void AssertCurrentPlayer(IPlayer expected)
        {
            _sut.Player.Should().Be(expected);
        }

        [Fact]
        public void Ends_turn_and_gets_next_turn()
        {
            _gameAdapter.Player.Returns(_nextPlayer);            

            var sut = _sut;
            sut.EndTurn();

            sut.Player.Should().Be(_nextPlayer);
        }

        [Fact]
        public void Show_game_over_when_game_is_updated()
        {
            _gameAdapter.Player.Returns(_currentPlayer);

            var gameOverViewModel = new GameOverViewModel(_currentPlayer);
            _gameOverViewModelFactory.Create(_currentPlayer).Returns(gameOverViewModel);

            _gameAdapter.IsGameOver().Returns(true);

            _sut.OnTerritoryClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            _gameAdapter.IsGameOver().Returns(false);
            var gameOverViewModel = new GameOverViewModel(_currentPlayer);

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

            _gameAdapter.Received().OnClick(_territory1);
        }

        [Fact]
        public void Fortifies_armies()
        {
            _sut.Fortify();

            _gameAdapter.Received().Fortify();
        }
    }
}