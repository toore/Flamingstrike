using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IEnumerable<ITerritoryGeography> _enabledTerritories;
        private readonly ITerritoryGeography _selectedTerritoryGeography;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IEnumerable<ITerritoryGeography> enabledTerritories, ITerritoryGeography selectedTerritoryGeography, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _territories = territories;
            _enabledTerritories = enabledTerritories;
            _selectedTerritoryGeography = selectedTerritoryGeography;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void Visit(TerritoryViewModel territoryViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(territoryViewModel.TerritoryGeography);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedTerritoryGeography == territoryViewModel.TerritoryGeography)
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
            return _enabledTerritories.Contains(territoryViewModel.TerritoryGeography);
        }

        public void Visit(TitleViewModel titleViewModel)
        {
            UpdateArmiesForTerritory(titleViewModel);
        }

        private void UpdateArmiesForTerritory(TitleViewModel titleViewModel)
        {
            var gameboardTerritory = _territories.Get(titleViewModel.TerritoryGeography);
            titleViewModel.Armies = gameboardTerritory.Armies;
        }
    }
}