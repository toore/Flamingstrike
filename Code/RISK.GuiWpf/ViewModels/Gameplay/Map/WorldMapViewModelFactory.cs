using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.Extensions;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> onClick);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly IWorldMapModelFactory _worldMapModelFactory;
        private readonly ITerritoryViewModelColorInitializer _territoryViewModelColorInitializer;

        public WorldMapViewModelFactory(IWorldMapModelFactory worldMapModelFactory, ITerritoryViewModelColorInitializer territoryViewModelColorInitializer)
        {
            _worldMapModelFactory = worldMapModelFactory;
            _territoryViewModelColorInitializer = territoryViewModelColorInitializer;
        }

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> onClick)
        {
            var territoryModels = _worldMapModelFactory.Create(worldMap);
            var worldMapItemViewModels = territoryModels.SelectMany(x=>CreateXamlModels(x, onClick)).ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModel.WorldMapViewModels.Add(worldMapItemViewModels);

            foreach (var worldMapItemViewModel in worldMapItemViewModels.OfType<ITerritoryLayoutViewModel>())
            {
                _territoryViewModelColorInitializer.UpdateColors(worldMap, worldMapItemViewModel);
            }

            return worldMapViewModel;
        }

        private IEnumerable<IWorldMapItemViewModel> CreateXamlModels(ITerritoryModel territoryModel, Action<ITerritory> onClick)
        {
            yield return new TerritoryLayoutViewModel(territoryModel, onClick);
            yield return new TerritoryTextViewModel(territoryModel);
        }
    }
}