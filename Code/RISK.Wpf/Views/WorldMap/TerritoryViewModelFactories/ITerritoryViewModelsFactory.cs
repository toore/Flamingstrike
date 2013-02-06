using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public interface ITerritoryViewModelsFactory
    {
        TextViewModel CreateTextViewModel(ITerritory territory);
        TerritoryViewModel CreateTerritoryViewModel();
    }
}