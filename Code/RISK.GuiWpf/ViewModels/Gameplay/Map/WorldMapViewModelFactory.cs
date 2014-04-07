using System;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        private readonly ITerritoryTextViewModelFactory _territoryTextViewModelFactory;
        private readonly Locations _locations;

        public WorldMapViewModelFactory(Locations locations, ITerritoryViewModelFactory territoryViewModelFactory, ITerritoryTextViewModelFactory territoryTextViewModelFactory)
        {
            _territoryViewModelFactory = territoryViewModelFactory;
            _territoryTextViewModelFactory = territoryTextViewModelFactory;
            _locations = locations;
        }

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ILocation> selectLocation)
        {
            var territories = _locations.GetAll()
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

        private IWorldMapItemViewModel CreateTerritoryViewModel(ITerritory territory, Action<ILocation> selectLocation)
        {
            return _territoryViewModelFactory.Create(territory, selectLocation);
        }

        private IWorldMapItemViewModel CreateTextViewModel(ITerritory territory)
        {
            return _territoryTextViewModelFactory.Create(territory);
        }
    }
}