using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface ITerritoryViewModelsFactory
    {
        TextViewModel CreateTextViewModel(ITerritory territory);
        TerritoryViewModel CreateTerritoryViewModel();
    }
}