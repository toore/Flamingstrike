using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface ITerritoryViewModelsFactorySelector
    {
        ITerritoryViewModelsFactory Select(ITerritory territory);
    }
}