using System;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IWorldMapModelFactory _worldMapModelFactory;

        public TerritoryViewModelFactory(ITerritoryViewModelUpdater territoryViewModelUpdater, IWorldMapModelFactory worldMapModelFactory)
        {
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _worldMapModelFactory = worldMapModelFactory;
        }

        public TerritoryLayoutViewModel Create(ITerritory territory, Action<ITerritory> clickCommand)
        {
            throw new NotSupportedException();
            //var territoryModel = _worldMapModelFactory.Create(territory);

            //var territoryViewModel = new TerritoryLayoutViewModel(territory, territoryModel.Path, clickCommand);
            //territoryViewModel.IsEnabled = true;
            //_territoryViewModelUpdater.UpdateColors(territoryViewModel, territory);

            //return territoryViewModel;
        }
    }
}