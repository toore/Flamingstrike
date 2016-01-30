using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IEnumerable<IRegion> _enabledTerritories;
        private readonly IRegion _selectedRegion;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IEnumerable<IRegion> enabledTerritories, IRegion selectedRegion, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _territories = territories;
            _enabledTerritories = enabledTerritories;
            _selectedRegion = selectedRegion;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void Visit(TerritoryViewModel territoryViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(territoryViewModel.Region);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedRegion == territoryViewModel.Region)
            {
                fillColor = _colorService.SelectedTerritoryColor;
            }

            territoryViewModel.StrokeColor = strokeColor;
            territoryViewModel.FillColor = fillColor;
            territoryViewModel.MouseOverStrokeColor = mouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = mouseOverFillColor;

            territoryViewModel.IsEnabled = IsTerritoryEnabled(territoryViewModel);
        }

        private bool IsTerritoryEnabled(TerritoryViewModel territoryViewModel)
        {
            return _enabledTerritories.Contains(territoryViewModel.Region);
        }

        public void Visit(TitleViewModel titleViewModel)
        {
            UpdateArmiesForTerritory(titleViewModel);
        }

        private void UpdateArmiesForTerritory(TitleViewModel titleViewModel)
        {
            var gameboardTerritory = _territories
                .Single(x => x.Region == titleViewModel.Region);

            titleViewModel.Armies = gameboardTerritory.Armies;
        }
    }
}