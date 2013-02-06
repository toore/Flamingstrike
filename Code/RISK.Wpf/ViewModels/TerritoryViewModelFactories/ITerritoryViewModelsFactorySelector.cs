using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.TerritoryViewModelFactories
{
    public interface ITerritoryViewModelsFactorySelector
    {
        ITerritoryViewModelsFactory Select(ITerritory territory);
    }
}