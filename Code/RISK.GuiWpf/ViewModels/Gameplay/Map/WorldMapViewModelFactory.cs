using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        private readonly ITerritoryTextViewModelFactory _territoryTextViewModelFactory;

        public WorldMapViewModelFactory( ITerritoryViewModelFactory territoryViewModelFactory, ITerritoryTextViewModelFactory territoryTextViewModelFactory)
        {
            _territoryViewModelFactory = territoryViewModelFactory;
            _territoryTextViewModelFactory = territoryTextViewModelFactory;
        }

        public WorldMapViewModel Create(IEnumerable<ITerritory> territories, Action<ITerritory> selectLocation)
        {
            var worldMapViewModels = territories
                .Select(x => CreateTerritoryViewModel(x, selectLocation))
                .Union(territories.Select(CreateTextViewModel))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            return worldMapViewModel;
        }

        private IWorldMapItemViewModel CreateTerritoryViewModel(ITerritory territory, Action<ITerritory> selectLocation)
        {
            return _territoryViewModelFactory.Create(territory, selectLocation);
        }

        private IWorldMapItemViewModel CreateTextViewModel(ITerritory territory)
        {
            return _territoryTextViewModelFactory.Create(territory);
        }
    }
}