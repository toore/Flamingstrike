using System;
using GuiWpf.Services;
using GuiWpf.Territories;
using RISK.Domain.Entities;

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

        public TerritoryLayoutViewModel Create(ITerritory territory, Action<ILocation> clickCommand)
        {
            var layoutInformation = _territoryGuiFactory.Create(territory.Location);

            var territoryViewModel = new TerritoryLayoutViewModel(territory.Location, layoutInformation.Path, clickCommand);
            territoryViewModel.IsEnabled = true;
            _territoryViewModelUpdater.UpdateColors(territoryViewModel, territory);

            return territoryViewModel;
        }
    }
}