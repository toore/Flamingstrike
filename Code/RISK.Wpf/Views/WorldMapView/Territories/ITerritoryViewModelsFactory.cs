using RISK.Domain.Entities;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface ITerritoryViewModelsFactory
    {
        TerritoryInformationViewModel CreateTextViewModel(ITerritory territory);
        TerritoryViewModel CreateTerritoryViewModel();
    }
}