using GuiWpf.TerritoryModels;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryTextViewModelFactory
    {
        ITerritoryTextViewModel Create(ITerritory territory);
    }

    public class TerritoryTextViewModelFactory : ITerritoryTextViewModelFactory
    {
        private readonly ITerritoryGuiFactory _territoryGuiFactory;

        public TerritoryTextViewModelFactory(ITerritoryGuiFactory territoryGuiFactory)
        {
            _territoryGuiFactory = territoryGuiFactory;
        }

        public ITerritoryTextViewModel Create(ITerritory territory)
        {
            var territoryModel = _territoryGuiFactory.Create(territory);

            return new TerritoryTextViewModel
                {
                    Territory = territory,
                    TerritoryName = territoryModel.Name,
                    Position = territoryModel.NamePosition,
                    Armies = territory.Armies
                };
        }
    }
}