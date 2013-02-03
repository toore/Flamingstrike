using System.Linq;
using GuiWpf.Views.WorldMapView.Territories;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapView
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelsFactorySelector _territoryViewModelsFactorySelector;
        private readonly ILocationRepository _locationRepository;

        public WorldMapViewModelFactory(ITerritoryViewModelsFactorySelector territoryViewModelsFactorySelector, ILocationRepository locationRepository)
        {
            _territoryViewModelsFactorySelector = territoryViewModelsFactorySelector;
            _locationRepository = locationRepository;
        }

        public WorldMapViewModel Create(IWorldMap worldMap)
        {
            var territories = _locationRepository.GetAll().Select(worldMap.GetTerritory).ToList();

            var worldMapViewModels = territories
                .Select(CreateTerritoryViewModel)
                .Union(territories.Select(CreateTextViewModel))
                .ToList();

            return new WorldMapViewModel
                {
                    WorldMapViewModels = worldMapViewModels
                };
        }

        private IWorldMapViewModel CreateTerritoryViewModel(ITerritory territory)
        {
            return GetFactory(territory).CreateTerritoryViewModel();
        }

        private IWorldMapViewModel CreateTextViewModel(ITerritory territory)
        {
            return GetFactory(territory).CreateTextViewModel(territory);
        }

        private ITerritoryViewModelsFactory GetFactory(ITerritory territory)
        {
            return _territoryViewModelsFactorySelector.Select(territory);
        }
    }
}