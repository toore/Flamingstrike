using GuiWpf.GuiDefinitions;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.WorldMap
{
    public class TextViewModelFactory : ITextViewModelFactory
    {
        private readonly ITerritoryGuiDefinitionFactory _territoryGuiDefinitionFactory;

        public TextViewModelFactory(ITerritoryGuiDefinitionFactory territoryGuiDefinitionFactory)
        {
            _territoryGuiDefinitionFactory = territoryGuiDefinitionFactory;
        }

        public TextViewModel Create(ITerritory territory)
        {
            var layoutInformation = _territoryGuiDefinitionFactory.Create(territory.Location);

            return new TextViewModel
                {
                    TerritoryName = layoutInformation.Name,
                    Position = layoutInformation.NamePosition
                };
        }
    }
}