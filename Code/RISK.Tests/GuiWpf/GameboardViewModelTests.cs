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
using RISK.Application.GamePlay;
using RISK.Application.World;
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
        private readonly IPlayerId _currentPlayerId;
        private readonly IPlayerId _nextPlayerId;
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

            _currentPlayerId = Substitute.For<IPlayerId>();
            _nextPlayerId = Substitute.For<IPlayerId>();

            _firstPlayerInteractionState = Substitute.For<IInteractionState>();
            _firstPlayerInteractionState.PlayerId.Returns(_currentPlayerId);
            _nextPlayerInteractionState = Substitute.For<IInteractionState>();
            _nextPlayerInteractionState.PlayerId.Returns(_nextPlayerId);

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

            //_worldMapViewModelFactory.Create(Arg.Is(worldMap), Arg.Any<Action<ITerritory>>(), Arg.Any<IEnumerable<ITerritory>>()).Returns(_worldMapViewModel);

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
            _gameAdapter.PlayerId.Returns(_currentPlayerId);

            _sut.Activate();

            AssertCurrentPlayer(_currentPlayerId);
        }

        [Fact]
        public void Next_player_takes_second_turn()
        {
            _gameAdapter.PlayerId.Returns(_nextPlayerId);

            _sut.EndTurn();

            AssertCurrentPlayer(_nextPlayerId);
        }

        private void AssertCurrentPlayer(IPlayerId expected)
        {
            _sut.PlayerId.Should().Be(expected);
        }

        [Fact]
        public void Ends_turn_and_gets_next_turn()
        {
            _gameAdapter.PlayerId.Returns(_nextPlayerId);            

            var sut = _sut;
            sut.EndTurn();

            sut.PlayerId.Should().Be(_nextPlayerId);
        }

        [Fact]
        public void Show_game_over_when_game_is_updated()
        {
            _gameAdapter.PlayerId.Returns(_currentPlayerId);

            var gameOverViewModel = new GameOverViewModel(_currentPlayerId);
            _gameOverViewModelFactory.Create(_currentPlayerId).Returns(gameOverViewModel);

            _gameAdapter.IsGameOver().Returns(true);

            _sut.OnTerritoryClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void When_game_is_not_over_no_game_over_dialog_should_be_shown()
        {
            _gameAdapter.IsGameOver().Returns(false);
            var gameOverViewModel = new GameOverViewModel(_currentPlayerId);

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