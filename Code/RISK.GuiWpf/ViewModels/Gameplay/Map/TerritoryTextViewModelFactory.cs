using GuiWpf.Territories;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModelFactory : ITerritoryTextViewModelFactory
    {
        private readonly ITerritoryGuiFactory _territoryGuiFactory;

        public TerritoryTextViewModelFactory(ITerritoryGuiFactory territoryGuiFactory)
        {
            _territoryGuiFactory = territoryGuiFactory;
        }

        public ITerritoryTextViewModel Create(ITerritory territory)
        {
            var layoutInformation = _territoryGuiFactory.Create(territory.Location);

            return new TerritoryTextViewModel
                {
                    Location = territory.Location,
                    TerritoryName = layoutInformation.Name,
                    Position = layoutInformation.NamePosition,
                    Armies = territory.Armies
                };
        }
    }
}