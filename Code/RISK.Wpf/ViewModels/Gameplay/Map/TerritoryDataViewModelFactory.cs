using GuiWpf.GuiDefinitions;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryDataViewModelFactory : ITerritoryDataViewModelFactory
    {
        private readonly ITerritoryGuiDefinitionFactory _territoryGuiDefinitionFactory;

        public TerritoryDataViewModelFactory(ITerritoryGuiDefinitionFactory territoryGuiDefinitionFactory)
        {
            _territoryGuiDefinitionFactory = territoryGuiDefinitionFactory;
        }

        public ITerritoryDataViewModel Create(ITerritory territory)
        {
            var layoutInformation = _territoryGuiDefinitionFactory.Create(territory.Location);

            return new TerritoryDataViewModel
                {
                    Location = territory.Location,
                    TerritoryName = layoutInformation.Name,
                    Position = layoutInformation.NamePosition
                };
        }
    }
}