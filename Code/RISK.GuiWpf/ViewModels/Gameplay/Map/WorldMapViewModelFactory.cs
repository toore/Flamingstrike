using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.Extensions;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> onClick, IEnumerable<ITerritory> enabledTerritories);
        void Update(WorldMapViewModel worldMapViewModel, IWorldMap worldMap, ITerritory selectedTerritory, IEnumerable<ITerritory> enabledTerritories);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IWorldMapModelFactory _worldMapModelFactory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(IWorldMapModelFactory worldMapModelFactory, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _worldMapModelFactory = worldMapModelFactory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> onClick, IEnumerable<ITerritory> enabledTerritories)
        {
            var territoryModels = _worldMapModelFactory.Create(worldMap);
            var worldMapItemViewModels = territoryModels
                .SelectMany(x => CreateXamlModels(x, onClick))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModel.WorldMapViewModels.Add(worldMapItemViewModels);

            Update(worldMapViewModel, worldMap, null, enabledTerritories);

            return worldMapViewModel;
        }

        public void Update(WorldMapViewModel worldMapViewModel, IWorldMap worldMap, ITerritory selectedTerritory, IEnumerable<ITerritory> enabledTerritories)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(worldMap, enabledTerritories, selectedTerritory, _territoryColorsFactory, _colorService);
            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }

        private static IEnumerable<IWorldMapItemViewModel> CreateXamlModels(ITerritoryModel territoryModel, Action<ITerritory> onClick)
        {
            yield return new TerritoryLayoutViewModel(territoryModel, onClick);
            yield return new TerritoryTextViewModel(territoryModel);
        }
    }
}