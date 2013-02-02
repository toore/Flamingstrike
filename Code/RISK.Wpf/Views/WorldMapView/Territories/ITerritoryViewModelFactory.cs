using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface ITerritoryViewModelFactory
    {
        TerritoryViewModelBase Create(ITerritory territory);
    }
}