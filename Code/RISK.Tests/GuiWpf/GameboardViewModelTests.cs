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
        private IGame _game;
        private WorldMapViewModel _worldMapViewModel;
        private ILocation _location1;
        private ILocation _location2;
        private ITerritoryLayoutViewModel _layoutViewModel1;
        private ITerritoryTextViewModel _textViewModel1;
        private ITerritoryLayoutViewModel _layoutViewModel2;
        private ITerritoryTextViewModel _textViewModel2;
        private IWorldMap _worldMap;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITurnState _initialTurnState;
        private ITurnState _nextTurnState;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private IPlayer _player1;
        private IPlayer _player2;
        private IGameOverEvaluater _gameOverEvaluater;
        private IWindowManager _windowManager;
        private IGameOverViewModelFactory _gameOverViewModelFactory;
        private ILanguageResources _languageResources;
        private IDialogManager _dialogManager;
        private IEventAggregator _gameEventAggregator;
        private ITurnPhaseFactory _turnPhaseFactory;
        private ILocation[] _locations;
        private IWorldMapViewModelFactory _worldMapViewModelFactory;

        public GameboardViewModelTests()
        {
            _game = Substitute.For<IGame>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _gameOverEvaluater = Substitute.For<IGameOverEvaluater>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _languageResources = Substitute.For<ILanguageResources>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            LanguageResources.Instance = _languageResources;

            _location1 = Substitute.For<ILocation>();
            _location2 = Substitute.For<ILocation>();
            _locations = new[]
            {
                _location1,
                _location2
            };

            _worldMap = Substitute.For<IWorldMap>();
            _territory1 = new Territory(_location1);
            _territory2 = new Territory(_location2);
            _worldMap.GetTerritory(_location1).Returns(_territory1);
            _worldMap.GetTerritory(_location2).Returns(_territory2);
            _game.GetWorldMap().Returns(_worldMap);

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();

            _initialTurnState = Substitute.For<ITurnState>();
            _initialTurnState.Player.Returns(_player1);
            _nextTurnState = Substitute.For<ITurnState>();
            _nextTurnState.Player.Returns(_player2);
            _game.GetNextTurn().Returns(_initialTurnState, _nextTurnState);

            _layoutViewModel1 = StubLayoutViewModel(_location1);
            _textViewModel1 = StubTextViewModel(_location1);
            _layoutViewModel2 = StubLayoutViewModel(_location2);
            _textViewModel2 = StubTextViewModel(_location2);

            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(_layoutViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(_textViewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(_layoutViewModel2);
            _worldMapViewModel.WorldMapViewModels.Add(_textViewModel2);

            _worldMapViewModelFactory.Create(Arg.Is(_worldMap), Arg.Any<Action<ILocation>>()).Returns(_worldMapViewModel);

            _turnPhaseFactory = Substitute.For<ITurnPhaseFactory>();
        }

        private GameboardViewModel Create()
        {
            return new GameboardViewModel(_game, _locations, _worldMapViewModelFactory, _territoryViewModelUpdater, _gameOverEvaluater, _windowManager,
                _gameOverViewModelFactory, _dialogManager, _gameEventAggregator, _turnPhaseFactory);
        }

        [Fact]
        public void Initializes_WorldMapViewModel()
        {
            Create().WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Player1_takes_first_turn()
        {
            AssertCurrentPlayer(_player1);
        }

        [Fact]
        public void Player2_takes_second_turn()
        {
            Create().EndTurn();

            AssertCurrentPlayer(_player2);
        }

        private void AssertCurrentPlayer(IPlayer expected)
        {
            Create().Player.Should().Be(expected);
        }

        [Fact]
        public void OnLocationClick_commands_select()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            _initialTurnState.CanSelect(_location1).Returns(true);

            Create().OnLocationClick(_location1);

            _initialTurnState.Received().Select(_location1);
        }

        [Fact]
        public void OnLocationClick_commands_attack()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            _initialTurnState.CanSelect(_location2).Returns(false);
            _initialTurnState.CanAttack(_location2).Returns(true);

            var sut = Create();
            sut.OnLocationClick(_location1);
            sut.OnLocationClick(_location2);

            _initialTurnState.Received().Attack(_location2);
        }

        [Fact]
        public void OnLocationClick_selects_territory()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            _initialTurnState.CanSelect(_location1).Returns(true);

            Create().OnLocationClick(_location1);

            _layoutViewModel1.IsSelected = true;
        }

        [Fact]
        public void Select_location_can_select_location_2()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            _initialTurnState.CanAttack(_location1).Returns(false);
            _initialTurnState.CanAttack(_location2).Returns(true);

            Create().OnLocationClick(_location1);

            _layoutViewModel1.IsEnabled.Should().BeFalse("location 1 can not be selected");
            _layoutViewModel2.IsEnabled.Should().BeTrue("location 2 can be selected");
        }

        [Fact]
        public void Ends_turn_and_gets_next_turn()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            var sut = Create();
            _game.ClearReceivedCalls();

            sut.EndTurn();

            _initialTurnState.Received(1).EndTurn();
            _game.Received(1).GetNextTurn();
        }

        [Fact]
        public void When_winning_game_over_dialog_should_be_shown()
        {
            _turnPhaseFactory.CreateAttackPhase(_initialTurnState).Returns(new AttackPhase(_initialTurnState));
            _gameOverEvaluater.IsGameOver(_worldMap).Returns(true);
            var gameOverViewModel = new GameOverViewModel(_player1);
            _gameOverViewModelFactory.Create(_player1).Returns(gameOverViewModel);

            Create().OnLocationClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Fact]
        public void End_game_shows_confirm_dialog()
        {
            Create().EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
        }

        [Fact]
        public void Can_fortify()
        {
            Create().CanFortify().Should().BeTrue();
        }

        [Fact]
        public void Can_not_fortify_when_already_fortifying()
        {
            var sut = Create();
            sut.Fortify();

            sut.CanFortify().Should().BeFalse();
        }

        [Fact]
        public void Fortifies()
        {
            _turnPhaseFactory.CreateFortifyingPhase(_initialTurnState).Returns(new FortifyingPhase(_initialTurnState));
            _initialTurnState.CanSelect(_location1).Returns(true);
            _initialTurnState.CanFortify(_location2).Returns(true);

            var sut = Create();
            sut.Fortify();

            sut.OnLocationClick(_location1);
            sut.OnLocationClick(_location2);

            _initialTurnState.Received(1).Fortify(_location2, 10);
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