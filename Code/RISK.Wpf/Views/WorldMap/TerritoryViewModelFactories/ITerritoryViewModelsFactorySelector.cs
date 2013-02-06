using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public interface ITerritoryViewModelsFactorySelector
    {
        ITerritoryViewModelsFactory Select(ITerritory territory);
    }
}