using System;
using GuiWpf.GuiDefinitions;
using GuiWpf.Services;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryViewModelFactory : ITerritoryViewModelFactory
    {
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly ITerritoryGuiDefinitionFactory _territoryGuiDefinitionFactory;

        public TerritoryViewModelFactory(ITerritoryViewModelUpdater territoryViewModelUpdater, ITerritoryGuiDefinitionFactory territoryGuiDefinitionFactory)
        {
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _territoryGuiDefinitionFactory = territoryGuiDefinitionFactory;
        }

        public TerritoryViewModel Create(ITerritory territory, Action<ILocation> clickCommand)
        {
            var layoutInformation = _territoryGuiDefinitionFactory.Create(territory.Location);

            var territoryViewModel = new TerritoryViewModel(territory.Location, layoutInformation.Path, clickCommand);
            territoryViewModel.IsEnabled = true;
            _territoryViewModelUpdater.UpdateColor(territoryViewModel, territory);

            return territoryViewModel;
        }
    }
}