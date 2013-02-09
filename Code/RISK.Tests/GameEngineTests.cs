using System;
using System.Collections.Generic;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels.WorldMapViewModels;
using GuiWpf.Views.WorldMapViews;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests
{
    [TestFixture]
    public class GameEngineTests
    {
        private GameEngine _gameEngine;
        private IGame _game;
        private ILocationRepository _locationRepository;
        private IWorldMapViewModelFactory _worldMapViewModelFactory;
        private WorldMapViewModel _worldMapViewModel;
        private List<ILocation> _allLocations;
        private ILocation _location1;
        private ILocation _location2;
        private ITerritoryViewModel _viewModel1;
        private ITerritoryViewModel _viewModel2;
        private IWorldMap _worldMap;
        private IPlayer _player1;
        private ITerritoryViewModelUpdater _territoryViewModelUpdater;

        [SetUp]
        public void SetUp()
        {
            _game = MockRepository.GenerateStub<IGame>();
            _locationRepository = MockRepository.GenerateStub<ILocationRepository>();
            _worldMapViewModelFactory = MockRepository.GenerateStub<IWorldMapViewModelFactory>();
            _territoryViewModelUpdater = MockRepository.GenerateStub<ITerritoryViewModelUpdater>();

            _location1 = MockRepository.GenerateStub<ILocation>();
            _location2 = MockRepository.GenerateStub<ILocation>();
            _allLocations = new List<ILocation>
                {
                    _location1,
                    _location2
                };
            _locationRepository.Stub(x => x.GetAll()).Return(_allLocations);

            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _game.Stub(x => x.GetWorldMap()).Return(_worldMap);

            _viewModel1 = StubWorldViewModel(_location1);
            _viewModel2 = StubWorldViewModel(_location2);
            _worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel1);
            _worldMapViewModel.WorldMapViewModels.Add(_viewModel2);
            _worldMapViewModelFactory.Stub(x => x.Create(Arg<IWorldMap>.Is.Equal(_worldMap), Arg<Action<ILocation>>.Is.Anything)).Return(_worldMapViewModel);

            _gameEngine = new GameEngine(_game, _locationRepository, _worldMapViewModelFactory, _territoryViewModelUpdater);

            _player1 = new HumanPlayer("Player 1");
        }

        private ITerritoryViewModel StubWorldViewModel(ILocation location)
        {
            var worldMapViewModel = MockRepository.GenerateStub<ITerritoryViewModel>();
            worldMapViewModel.Stub(x => x.Location).Return(location);
            return worldMapViewModel;
        }

        [Test]
        public void Initializes_WorldMapViewModel()
        {
            _gameEngine.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Test]
        public void SelectLocation_updates_viewmodel()
        {
            //var territory1 = MockRepository.GenerateStub<ITerritory>();
            //territory1.Stub(x => x.Owner).Return(_player1);
            //territory1.Armies = 1;

            //_worldMap.Stub(x => x.GetTerritory(_location1)).Return(territory1);

            //_gameEngine.SelectLocation(_location1);

            //_viewModel1.
        }
    }
}