using System;
using System.Collections.Generic;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.Gameplay
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
        private ITerritoryViewModel _viewModel1;
        private ITerritoryViewModel _viewModel2;
        private IWorldMap _worldMap;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITurn _turn1;
        private ITurn _turn2;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private IPlayer _player1;
        private IPlayer _player2;

        [SetUp]
        public void SetUp()
        {
            _game = Substitute.For<IGame>();
            _locationProvider = Substitute.For<ILocationProvider>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = Substitute.For<ITerritoryViewModelUpdater>();

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
            
            _turn1 = Substitute.For<ITurn>();
            _turn1.Player.Returns(_player1);
            _turn2 = Substitute.For<ITurn>();
            _turn2.Player.Returns(_player2);
            _game.GetNextTurn().Returns(_turn1, _turn2);

            _viewModel1 = StubWorldViewModel(_location1);
            _viewModel2 = StubWorldViewModel(_location2);
            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel2);
            
            _worldMapViewModelFactory.Create(Arg.Is(_worldMap), Arg.Any<Action<ILocation>>()).Returns(_worldMapViewModel);

            _gameboardViewModel = new GameboardViewModel(_game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater);
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
        public void SelectLocation_invokes_turn_select()
        {
            _gameboardViewModel.SelectLocation(_location1);

            _turn1.Received().Select(_location1);
        }

        [Test]
        public void SelectLocation_invokes_turn_attack()
        {
            _turn1.IsTerritorySelected.Returns(true);

            _gameboardViewModel.SelectLocation(_location2);

            _turn1.Received().Attack(_location2);
        }

        [Test]
        public void Ends_turn_and_gets_next_turn()
        {
            _game.ClearReceivedCalls();

            _gameboardViewModel.EndTurn();

            _turn1.Received(1).EndTurn();
            _game.Received(1).GetNextTurn();
        }

        private ITerritoryViewModel StubWorldViewModel(ILocation location)
        {
            var worldMapViewModel = Substitute.For<ITerritoryViewModel>();
            worldMapViewModel.Location.Returns(location);
            return worldMapViewModel;
        }
    }
}