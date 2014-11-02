using System;
using GuiWpf.Services;
using GuiWpf.Territories;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly ITerritoryGuiFactory _territoryGuiFactory;

        public TerritoryViewModelFactory(ITerritoryViewModelUpdater territoryViewModelUpdater, ITerritoryGuiFactory territoryGuiFactory)
        {
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _territoryGuiFactory = territoryGuiFactory;
        }

        public TerritoryLayoutViewModel Create(ITerritory territory, Action<ITerritory> clickCommand)
        {
            var layoutInformation = _territoryGuiFactory.Create(territory);

            var territoryViewModel = new TerritoryLayoutViewModel(territory, layoutInformation.Path, clickCommand);
            territoryViewModel.IsEnabled = true;
            _territoryViewModelUpdater.UpdateColors(territoryViewModel, territory);

            return territoryViewModel;
        }
    }
}