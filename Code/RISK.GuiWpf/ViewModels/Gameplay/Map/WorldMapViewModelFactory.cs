using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.Extensions;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<ITerritoryId> onClick, IEnumerable<ITerritoryId> enabledTerritories);
        void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, ITerritoryId selectedTerritoryId, IEnumerable<ITerritoryId> enabledTerritories);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IWorldMap _worldMap;
        private readonly IWorldMapModelFactory _worldMapModelFactory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(IWorldMap worldMap, IWorldMapModelFactory worldMapModelFactory, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _worldMap = worldMap;
            _worldMapModelFactory = worldMapModelFactory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(IReadOnlyList<ITerritory> territories, Action<ITerritoryId> onClick, IEnumerable<ITerritoryId> enabledTerritories)
        {
            var territoryModels = _worldMapModelFactory.Create(_worldMap);

            var worldMapViewModels = territoryModels
                .SelectMany(x => CreateViewModels(x, onClick))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModel.WorldMapViewModels.Add(worldMapViewModels);

            Update(worldMapViewModel, territories, null, enabledTerritories);

            return worldMapViewModel;
        }

        public void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<ITerritory> territories, ITerritoryId selectedTerritoryId, IEnumerable<ITerritoryId> enabledTerritories)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(territories, enabledTerritories, selectedTerritoryId, _territoryColorsFactory, _colorService);
            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }

        private static IEnumerable<IWorldMapItemViewModel> CreateViewModels(ITerritoryModel territoryModel, Action<ITerritoryId> onClick)
        {
            yield return new TerritoryViewModel(territoryModel, onClick);
            yield return new TitleViewModel(territoryModel);
        }
    }
}