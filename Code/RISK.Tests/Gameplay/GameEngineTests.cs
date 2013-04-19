using System;
using System.Collections.Generic;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.WorldMap;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class GameEngineTests
    {
        private GameEngine _gameEngine;
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
        private ITurn _turn;
        private ITerritory _territory1;
        private ITerritory _territory2;

        [SetUp]
        public void SetUp()
        {
            _game = MockRepository.GenerateStub<IGame>();
            _locationProvider = MockRepository.GenerateStub<ILocationProvider>();
            _worldMapViewModelFactory = MockRepository.GenerateStub<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = MockRepository.GenerateStub<ITerritoryViewModelUpdater>();

            _location1 = MockRepository.GenerateStub<ILocation>();
            _location2 = MockRepository.GenerateStub<ILocation>();
            _allLocations = new List<ILocation>
                {
                    _location1,
                    _location2
                };
            _locationProvider.Stub(x => x.GetAll()).Return(_allLocations);

            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _territory1 = new Territory(_location1);
            _territory2 = new Territory(_location2);
            _worldMap.Stub(x => x.GetTerritory(_location1)).Return(_territory1);
            _worldMap.Stub(x => x.GetTerritory(_location2)).Return(_territory2);
            _game.Stub(x => x.GetWorldMap()).Return(_worldMap);
            _turn = MockRepository.GenerateStub<ITurn>();
            _game.Stub(x => x.GetNextTurn()).Return(_turn);

            _viewModel1 = StubWorldViewModel(_location1);
            _viewModel2 = StubWorldViewModel(_location2);
            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel2);
            _worldMapViewModelFactory.Stub(x => x.Create(Arg<IWorldMap>.Is.Equal(_worldMap), Arg<Action<ILocation>>.Is.Anything)).Return(_worldMapViewModel);

            _gameEngine = new GameEngine(_game, _locationProvider, _worldMapViewModelFactory, _territoryViewModelUpdater);
        }

        [Test]
        public void Initializes_WorldMapViewModel()
        {
            _gameEngine.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Test]
        public void SelectLocation_invokes_turn_select()
        {
            _gameEngine.SelectLocation(_location1);

            _turn.AssertWasCalled(x => x.Select(_location1));
        }

        [Test]
        public void SelectLocation_invokes_turn_attack()
        {
            _turn.Stub(x => x.IsTerritorySelected).Return(true);

            _gameEngine.SelectLocation(_location2);

            _turn.AssertWasCalled(x => x.Attack(_location2));
        }

        private ITerritoryViewModel StubWorldViewModel(ILocation location)
        {
            var worldMapViewModel = MockRepository.GenerateStub<ITerritoryViewModel>();
            worldMapViewModel.Stub(x => x.Location).Return(location);
            return worldMapViewModel;
        }
    }
}