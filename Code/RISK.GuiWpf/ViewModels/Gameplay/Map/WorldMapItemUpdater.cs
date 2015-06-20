using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IGameboard _gameboard;
        private readonly IEnumerable<ITerritory> _enabledTerritories;
        private readonly ITerritory _selectedTerritory;
        private readonly ITerritoryColorsFactory _territoryColorsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IGameboard gameboard, IEnumerable<ITerritory> enabledTerritories, ITerritory selectedTerritory, ITerritoryColorsFactory territoryColorsFactory, IColorService colorService)
        {
            _gameboard = gameboard;
            _enabledTerritories = enabledTerritories;
            _selectedTerritory = selectedTerritory;
            _territoryColorsFactory = territoryColorsFactory;
            _colorService = colorService;
        }

        public void Visit(TerritoryViewModel territoryViewModel)
        {
            var territoryColors = _territoryColorsFactory.Create(territoryViewModel.Territory);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedTerritory == territoryViewModel.Territory)
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
            return _enabledTerritories.Contains(territoryViewModel.Territory);
        }

        public void Visit(TitleViewModel titleViewModel)
        {
            UpdateArmiesForTerritory(titleViewModel);
        }

        private void UpdateArmiesForTerritory(TitleViewModel titleViewModel)
        {
            var gameboardTerritory = _gameboard.GetTerritory(titleViewModel.Territory);
            titleViewModel.Armies = gameboardTerritory.Armies;
        }
    }
}