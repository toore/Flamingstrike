using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.Extensions;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<IRegion> onClick, IEnumerable<IRegion> enabledTerritories);
        void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IRegion selectedRegion, IEnumerable<IRegion> enabledTerritories);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IRegions _regions;
        private readonly IWorldMapModelFactory _worldMapModelFactory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(IRegions regions, IWorldMapModelFactory worldMapModelFactory, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _regions = regions;
            _worldMapModelFactory = worldMapModelFactory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<IRegion> onClick, IEnumerable<IRegion> enabledTerritories)
        {
            var territoryModels = _worldMapModelFactory.Create(_regions);

            var worldMapViewModels = territoryModels
                .SelectMany(x => CreateViewModels(x, onClick))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModel.WorldMapViewModels.Add(worldMapViewModels);

            Update(worldMapViewModel, territories, null, enabledTerritories);

            return worldMapViewModel;
        }

        public void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, IRegion selectedRegion, IEnumerable<IRegion> enabledTerritories)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(territories, enabledTerritories, selectedRegion, _territoryColorsFactory, _colorService);
            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }

        private static IEnumerable<IWorldMapItemViewModel> CreateViewModels(IRegionModel regionModel, Action<IRegion> onClick)
        {
            yield return new RegionViewModel(regionModel, onClick);
            yield return new TitleViewModel(regionModel);
        }
    }
}