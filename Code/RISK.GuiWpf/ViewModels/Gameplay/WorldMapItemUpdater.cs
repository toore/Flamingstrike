using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IReadOnlyList<IRegion> _enabledTerritories;
        private readonly IRegion _selectedRegion;
        private readonly IRegionColorSettingsFactory _regionColorSettingsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledTerritories, IRegion selectedRegion, IRegionColorSettingsFactory regionColorSettingsFactory, IColorService colorService)
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
            var fillColor = _selectedRegion == regionViewModel.Region ?
                _colorService.SelectedTerritoryColor : territoryColors.NormalFillColor;
            var mouseOverStrokeColor = territoryColors.MouseOverStrokeColor;
            var mouseOverFillColor = territoryColors.MouseOverFillColor;

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
            var territory = _territories
                .Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = territory.Armies;
        }
    }
}