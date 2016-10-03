using System;
using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine;
using RISK.UI.WPF.Extensions;
using RISK.UI.WPF.RegionModels;
using RISK.UI.WPF.Services;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(Action<IRegion> onClick);

        void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, IRegion selectedRegion);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IRegionModelFactory _regionModelFactory;
        private readonly IRegionColorSettingsFactory _regionColorSettingsFactory;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(IRegionModelFactory regionModelFactory, IRegionColorSettingsFactory regionColorSettingsFactory, IColorService colorService)
        {
            _regionModelFactory = regionModelFactory;
            _regionColorSettingsFactory = regionColorSettingsFactory;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(Action<IRegion> onClick)
        {
            var worldMapViewModel = new WorldMapViewModel();
            var worldMapViewModels = CreateWorldMapItemViewModels(onClick);
            worldMapViewModel.WorldMapViewModels.Add(worldMapViewModels);

            return worldMapViewModel;
        }

        public void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, IRegion selectedRegion)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(territories, enabledRegions, selectedRegion, _regionColorSettingsFactory, _colorService);
            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }

        private IEnumerable<IWorldMapItemViewModel> CreateWorldMapItemViewModels(Action<IRegion> onClick)
        {
            var regionModels = _regionModelFactory.Create().ToList();
            IEnumerable<IWorldMapItemViewModel> regionViewModels = regionModels.Select(regionModel => new RegionViewModel(regionModel, onClick));
            IEnumerable<IWorldMapItemViewModel> titleViewModels = regionModels.Select(regionModel => new RegionNameViewModel(regionModel));

            var worldMapViewModels = regionViewModels
                .Concat(titleViewModels)
                .ToList();

            return worldMapViewModels;
        }
    }
}