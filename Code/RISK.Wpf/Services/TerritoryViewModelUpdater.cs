using GuiWpf.ViewModels.Gameplay.Map;
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

        public void UpdateColor(ITerritoryLayoutViewModel territoryLayoutViewModel, ITerritory territory)
        {
            var territoryColors = _territoryColorsFactory.Create(territory);

            territoryLayoutViewModel.NormalStrokeColor = territoryColors.NormalStrokeColor;
            territoryLayoutViewModel.NormalFillColor = territoryColors.NormalFillColor;
            territoryLayoutViewModel.MouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            territoryLayoutViewModel.MouseOverFillColor = territoryColors.MouseOverFillColor;
        }
    }
}