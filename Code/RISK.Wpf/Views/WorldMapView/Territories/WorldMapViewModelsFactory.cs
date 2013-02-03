using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class WorldMapViewModelsFactory : IWorldMapViewModelsFactory
    {
        private readonly ITerritoryViewModelsFactorySelector _territoryViewModelsFactorySelector;
        private readonly IWorldMap _worldMap;
        private readonly ILocationRepository _locationRepository;

        public WorldMapViewModelsFactory(ITerritoryViewModelsFactorySelector territoryViewModelsFactorySelector, IWorldMap worldMap, ILocationRepository locationRepository)
        {
            _territoryViewModelsFactorySelector = territoryViewModelsFactorySelector;
            _worldMap = worldMap;
            _locationRepository = locationRepository;
        }

        public IEnumerable<IWorldMapViewModel> Create()
        {
            var territories = _locationRepository.GetAll().Select(_worldMap.GetTerritory).ToList();

            return territories.Select(CreateTerritoryViewModel)
                .Union(territories.Select(CreateTextViewModel))
                .ToList();
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