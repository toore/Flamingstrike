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
        private readonly IEnumerable<ITerritoryId> _enabledTerritories;
        private readonly ITerritoryId _selectedTerritoryId;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IEnumerable<ITerritoryId> enabledTerritories, ITerritoryId selectedTerritoryId, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _territories = territories;
            _enabledTerritories = enabledTerritories;
            _selectedTerritoryId = selectedTerritoryId;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void Visit(TerritoryViewModel territoryViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(territoryViewModel.TerritoryId);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedTerritoryId == territoryViewModel.TerritoryId)
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
            return _enabledTerritories.Contains(territoryViewModel.TerritoryId);
        }

        public void Visit(TitleViewModel titleViewModel)
        {
            UpdateArmiesForTerritory(titleViewModel);
        }

        private void UpdateArmiesForTerritory(TitleViewModel titleViewModel)
        {
            var gameboardTerritory = _territories.Get(titleViewModel.TerritoryId);
            titleViewModel.Armies = gameboardTerritory.Armies;
        }
    }
}