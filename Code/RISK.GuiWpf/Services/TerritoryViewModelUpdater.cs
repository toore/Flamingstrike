using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application;

namespace GuiWpf.Services
{
    public interface ITerritoryViewModelColorInitializer
    {
        void UpdateColors(IWorldMap worldMap, ITerritoryLayoutViewModel territoryLayoutViewModel);
    }

    public class TerritoryViewModelColorInitializer : ITerritoryViewModelColorInitializer
    {
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public TerritoryViewModelColorInitializer(ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void UpdateColors(IWorldMap worldMap, ITerritoryLayoutViewModel territoryLayoutViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(worldMap, territoryLayoutViewModel.Territory);

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