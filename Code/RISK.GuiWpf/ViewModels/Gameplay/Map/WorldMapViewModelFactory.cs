using System;
using RISK.Application;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> selectTerritory);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        //private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        //private readonly ITerritoryTextViewModelFactory _territoryTextViewModelFactory;

        //public WorldMapViewModelFactory( ITerritoryViewModelFactory territoryViewModelFactory, ITerritoryTextViewModelFactory territoryTextViewModelFactory)
        //{
        //    _territoryViewModelFactory = territoryViewModelFactory;
        //    _territoryTextViewModelFactory = territoryTextViewModelFactory;
        //}

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> selectTerritory)
        {
            //var worldMapViewModels = territories
            //    .Select(x => CreateTerritoryViewModel(x, selectTerritory))
            //    .Union(territories.Select(CreateTextViewModel))
            //    .ToList();

            //var worldMapViewModel = new WorldMapViewModel();
            //worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            //return worldMapViewModel;
            var worldMapViewModel = new WorldMapViewModel();

            //worldMap.

            return worldMapViewModel;
        }

        //private IWorldMapItemViewModel CreateTerritoryViewModel(ITerritory territory, Action<ITerritory> selectTerritory)
        //{
        //   return _territoryViewModelFactory.Create(territory, selectTerritory);
        //}

        //private IWorldMapItemViewModel CreateTextViewModel(ITerritory territory)
        //{
        //    return _territoryTextViewModelFactory.Create(territory);
        //}
    }
}