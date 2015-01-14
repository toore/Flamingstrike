using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Application;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IWorldMap _worldMap;
        private readonly IEnumerable<ITerritory> _enabledTerritories;
        private readonly ITerritory _selectedTerritory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(
            IWorldMap worldMap, 
            IEnumerable<ITerritory> enabledTerritories, 
            ITerritory selectedTerritory, 
            ITerritoryColorsFactory territoryColorsFactory, 
            IColorService colorService)
        {
            _worldMap = worldMap;
            _enabledTerritories = enabledTerritories;
            _selectedTerritory = selectedTerritory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void Visit(TerritoryLayoutViewModel territoryLayoutViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(_worldMap, territoryLayoutViewModel.Territory);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedTerritory == territoryLayoutViewModel.Territory)
            {
                fillColor = _colorService.SelectedTerritoryColor;
            }

            territoryLayoutViewModel.StrokeColor = strokeColor;
            territoryLayoutViewModel.FillColor = fillColor;
            territoryLayoutViewModel.MouseOverStrokeColor = mouseOverStrokeColor;
            territoryLayoutViewModel.MouseOverFillColor = mouseOverFillColor;

            territoryLayoutViewModel.IsEnabled = IsTerritoryEnabled(territoryLayoutViewModel);
        }

        private bool IsTerritoryEnabled(TerritoryLayoutViewModel territoryLayoutViewModel)
        {
            return _enabledTerritories.Contains(territoryLayoutViewModel.Territory);
        }

        public void Visit(TerritoryTextViewModel territoryTextViewModel)
        {
            territoryTextViewModel.UpdateArmies();
        }
    }
}