using System;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        private readonly ITerritoryDataViewModelFactory _territoryDataViewModelFactory;
        private readonly ILocationProvider _locationProvider;

        public WorldMapViewModelFactory(ILocationProvider locationProvider, ITerritoryViewModelFactory territoryViewModelFactory, ITerritoryDataViewModelFactory territoryDataViewModelFactory)
        {
            _territoryViewModelFactory = territoryViewModelFactory;
            _territoryDataViewModelFactory = territoryDataViewModelFactory;
            _locationProvider = locationProvider;
        }

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ILocation> selectLocation)
        {
            var territories = _locationProvider.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            var worldMapViewModels = territories
                .Select(x => CreateTerritoryViewModel(x, selectLocation))
                .Union(territories.Select(CreateTextViewModel))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            return worldMapViewModel;
        }

        private IWorldMapViewModel CreateTerritoryViewModel(ITerritory territory, Action<ILocation> selectLocation)
        {
            return _territoryViewModelFactory.Create(territory, selectLocation);
        }

        private IWorldMapViewModel CreateTextViewModel(ITerritory territory)
        {
            return _territoryDataViewModelFactory.Create(territory);
        }
    }
}