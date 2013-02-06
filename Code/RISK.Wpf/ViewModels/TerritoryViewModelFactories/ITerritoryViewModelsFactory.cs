using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public interface ITerritoryViewModelsFactory
    {
        TextViewModel CreateTextViewModel(ITerritory territory);
        TerritoryViewModel CreateTerritoryViewModel();
    }
}