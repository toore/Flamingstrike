using GuiWpf.ViewModels;
using GuiWpf.ViewModels.TerritoryViewModelFactories;
using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMap
{
    public class TextViewModelFactory : ITextViewModelFactory
    {
        private readonly ITerritoryLayoutInformationFactory _territoryLayoutInformationFactory;

        public TextViewModelFactory(ITerritoryLayoutInformationFactory territoryLayoutInformationFactory)
        {
            _territoryLayoutInformationFactory = territoryLayoutInformationFactory;
        }

        public TextViewModel Create(ITerritory territory)
        {
            var layoutInformation = _territoryLayoutInformationFactory.Create(territory.Location);

            return new TextViewModel
                {
                    TerritoryName = layoutInformation.Name,
                    Position = layoutInformation.NamePosition
                };
        }
    }
}