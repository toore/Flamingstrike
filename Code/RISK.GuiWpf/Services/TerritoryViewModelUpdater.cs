using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application.Entities;

namespace GuiWpf.Services
{
    public class TerritoryViewModelUpdater : ITerritoryViewModelUpdater
    {
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public TerritoryViewModelUpdater(ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void UpdateColors(ITerritoryLayoutViewModel territoryLayoutViewModel, ITerritory territory)
        {
            var territoryColors = _territoryColorsFactory.Create(territory);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (territoryLayoutViewModel.IsSelected)
            {
                fillColor = _colorService.SelectedTerritoryColor;
            }

            territoryLayoutViewModel.NormalStrokeColor = strokeColor;
            territoryLayoutViewModel.NormalFillColor = fillColor;
            territoryLayoutViewModel.MouseOverStrokeColor = mouseOverStrokeColor;
            territoryLayoutViewModel.MouseOverFillColor = mouseOverFillColor;
        }
    }
}