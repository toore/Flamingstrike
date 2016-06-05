using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IEnumerable<IRegion> _enabledTerritories;
        private readonly IRegion _selectedRegion;
        private readonly IRegionColorSettingsFactory _regionColorSettingsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IEnumerable<IRegion> enabledTerritories, IRegion selectedRegion, IRegionColorSettingsFactory regionColorSettingsFactory, IColorService colorService)
        {
            _territories = territories;
            _enabledTerritories = enabledTerritories;
            _selectedRegion = selectedRegion;
            _regionColorSettingsFactory = regionColorSettingsFactory;
            _colorService = colorService;
        }

        public void Visit(RegionViewModel regionViewModel)
        {
            var territoryColors = _regionColorSettingsFactory.Create(regionViewModel.Region);

            var strokeColor = territoryColors.NormalStrokeColor;
            var fillColor = territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

            if (_selectedRegion == regionViewModel.Region)
            {
                fillColor = _colorService.SelectedTerritoryColor;
            }

            regionViewModel.StrokeColor = strokeColor;
            regionViewModel.FillColor = fillColor;
            regionViewModel.MouseOverStrokeColor = mouseOverStrokeColor;
            regionViewModel.MouseOverFillColor = mouseOverFillColor;

            regionViewModel.IsEnabled = IsTerritoryEnabled(regionViewModel);
        }

        private bool IsTerritoryEnabled(RegionViewModel regionViewModel)
        {
            return _enabledTerritories.Contains(regionViewModel.Region);
        }

        public void Visit(RegionNameViewModel regionNameViewModel)
        {
            UpdateArmiesForTerritory(regionNameViewModel);
        }

        private void UpdateArmiesForTerritory(RegionNameViewModel regionNameViewModel)
        {
            var gameboardTerritory = _territories
                .Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = gameboardTerritory.Armies;
        }
    }
}