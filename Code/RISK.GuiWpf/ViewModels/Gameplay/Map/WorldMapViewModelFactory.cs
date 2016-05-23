using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.Extensions;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<IRegion> onClick, IEnumerable<IRegion> enabledTerritories);
        void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IRegion selectedRegion, IEnumerable<IRegion> enabledTerritories);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IRegionModelFactory _regionModelFactory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(IRegionModelFactory regionModelFactory, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _regionModelFactory = regionModelFactory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<IRegion> onClick, IEnumerable<IRegion> enabledTerritories)
        {
            var worldMapViewModel = new WorldMapViewModel();
            var worldMapViewModels = CreateWorldMapItemViewModels(onClick);
            worldMapViewModel.WorldMapViewModels.Add(worldMapViewModels);

            Update(worldMapViewModel, territories, null, enabledTerritories);

            return worldMapViewModel;
        }

        private IEnumerable<IWorldMapItemViewModel> CreateWorldMapItemViewModels(Action<IRegion> onClick)
        {
            var regionModels = _regionModelFactory.Create().ToList();
            IEnumerable<IWorldMapItemViewModel> regionViewModels = regionModels.Select(regionModel => new RegionOutlineViewModel(regionModel, onClick));
            IEnumerable<IWorldMapItemViewModel> titleViewModels = regionModels.Select(regionModel => new RegionNameViewModel(regionModel));

            var worldMapViewModels = regionViewModels
                .Concat(titleViewModels)
                .ToList();

            return worldMapViewModels;
        }

        public void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IRegion selectedRegion, IEnumerable<IRegion> enabledTerritories)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(territories, enabledTerritories, selectedRegion, _territoryColorsFactory, _colorService);
            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }
    }
}