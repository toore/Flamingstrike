using System;
using System.Collections.Generic;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class GameboardViewModelTests
    {
        private GameboardViewModel _gameboardViewModel;
        private IGame _game;
        private ILocationProvider _locationProvider;
        private IWorldMapViewModelFactory _worldMapViewModelFactory;
        private WorldMapViewModel _worldMapViewModel;
        private List<ILocation> _allLocations;
        private ILocation _location1;
        private ILocation _location2;
        private ITerritoryLayoutViewModel _layoutViewModel1;
        private ITerritoryTextViewModel _textViewModel1;
        private ITerritoryLayoutViewModel _layoutViewModel2;
        private ITerritoryTextViewModel _textViewModel2;
        private IWorldMap _worldMap;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITurn _currentTurn;
        private ITurn _nextTurn;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private IPlayer _player1;
        private IPlayer _player2;
        private IGameOverEvaluater _gameOverEvaluater;
        private IWindowManager _windowManager;
        private IGameOverViewModelFactory _gameOverViewModelFactory;
        private IResourceManagerWrapper _resourceManagerWrapper;
        private IDialogManager _dialogManager;
        private IGameEventAggregator _gameEventAggregator;

        [SetUp]
        public void SetUp()
        {
            _game = Substitute.For<IGame>();
            _locationProvider = Substitute.For<ILocationProvider>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();
            _gameOverEvaluater = Substitute.For<IGameOverEvaluater>();
            _windowManager = Substitute.For<IWindowManager>();
            _gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _resourceManagerWrapper = Substitute.For<IResourceManagerWrapper>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IGameEventAggregator>();

            _location1 = Substitute.For<ILocation>();
            _location2 = Substitute.For<ILocation>();
            _allLocations = new List<ILocation>
                {
                    _location1,
                    _location2
                };
            _locationProvider.GetAll().Returns(_allLocations);

            _worldMap = Substitute.For<IWorldMap>();
            _territory1 = new Territory(_location1);
            _territory2 = new Territory(_location2);
            _worldMap.GetTerritory(_location1).Returns(_territory1);
            _worldMap.GetTerritory(_location2).Returns(_territory2);
            _game.GetWorldMap().Returns(_worldMap);

            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            
            _currentTurn = Substitute.For<ITurn>();
            _currentTurn.Player.Returns(_player1);
            _nextTurn = Substitute.For<ITurn>();
            _nextTurn.Player.Returns(_player2);
            _game.GetNextTurn().Returns(_currentTurn, _nextTurn);

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

            _gameboardViewModel = new GameboardViewModel(_game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater, _gameOverEvaluater, _windowManager, _gameOverViewModelFactory, _resourceManagerWrapper, _dialogManager, _gameEventAggregator);
        }

        [Test]
        public void Initializes_WorldMapViewModel()
        {
            _gameboardViewModel.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Test]
        public void Player1_takes_first_turn()
        {
            AssertPlayer(_player1);
        }

        [Test]
        public void Player2_takes_second_turn()
        {
            _gameboardViewModel.EndTurn();

            AssertPlayer(_player2);
        }

        private void AssertPlayer(IPlayer expected)
        {
            _gameboardViewModel.Player.Should().Be(expected);
        }

        [Test]
        public void OnLocationClick_invokes_turn_select()
        {
            _currentTurn.CanSelect(_location1).Returns(true);

            _gameboardViewModel.OnLocationClick(_location1);

            _currentTurn.Received().Select(_location1);
        }

        [Test]
        public void OnLocationClick_invokes_turn_attack()
        {
            _currentTurn.CanSelect(_location2).Returns(false);
            _currentTurn.CanAttack(_location2).Returns(true);

            _gameboardViewModel.OnLocationClick(_location2);

            _currentTurn.Received().Attack(_location2);
        }

        [Test]
        public void OnLocationClick_selects_territory()
        {
            _gameboardViewModel.OnLocationClick(_location1);

            _layoutViewModel1.IsSelected = true;
        }

        [Test]
        public void Select_location_can_select_location_2()
        {
            _currentTurn.CanAttack(_location1).Returns(false);
            _currentTurn.CanAttack(_location2).Returns(true);

            _gameboardViewModel.OnLocationClick(_location1);

            _layoutViewModel1.IsEnabled.Should().BeFalse("location 1 can not be selected");
            _layoutViewModel2.IsEnabled.Should().BeTrue("location 1 can be selected");
        }

        [Test]
        public void Ends_turn_and_gets_next_turn()
        {
            _game.ClearReceivedCalls();

            _gameboardViewModel.EndTurn();

            _currentTurn.Received(1).EndTurn();
            _game.Received(1).GetNextTurn();
        }

        [Test]
        public void When_winning_game_over_dialog_should_be_shown()
        {
            _gameOverEvaluater.IsGameOver(_worldMap).Returns(true);
            var gameOverViewModel = new GameOverViewModel(_player1);
            _gameOverViewModelFactory.Create(_player1).Returns(gameOverViewModel);

            _gameboardViewModel.OnLocationClick(null);

            _windowManager.Received().ShowDialog(gameOverViewModel);
        }

        [Test]
        public void End_game_shows_confirm_dialog()
        {
            _gameboardViewModel.EndGame();

            _dialogManager.Received(1).ConfirmEndGame();
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