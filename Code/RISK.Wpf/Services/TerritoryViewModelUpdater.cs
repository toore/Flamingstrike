using GuiWpf.ViewModels.Gameplay.WorldMap;
using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public class TerritoryViewModelUpdater : ITerritoryViewModelUpdater
    {
        private readonly ITerritoryColorsFactory _territoryColorsFactory;

        public TerritoryViewModelUpdater(ITerritoryColorsFactory territoryColorsFactory)
        {
            _territoryColorsFactory = territoryColorsFactory;
        }

        public void UpdateColor(ITerritoryViewModel territoryViewModel, ITerritory territory)
        {
            var territoryColors = _territoryColorsFactory.Create(territory);

            territoryViewModel.NormalStrokeColor = territoryColors.NormalStrokeColor;
            territoryViewModel.NormalFillColor = territoryColors.NormalFillColor;
            territoryViewModel.MouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = territoryColors.MouseOverFillColor;
        }
    }
}