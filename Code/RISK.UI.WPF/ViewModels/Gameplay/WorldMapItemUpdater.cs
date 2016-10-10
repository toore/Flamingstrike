using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine;
using RISK.UI.WPF.Services;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IReadOnlyList<IRegion> _enabledRegions;
        private readonly Maybe<IRegion> _selectedRegion;
        private readonly IRegionColorSettingsFactory _regionColorSettingsFactory;
        private readonly IColorService _colorService;

        public WorldMapItemUpdater(IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, Maybe<IRegion> selectedRegion, IRegionColorSettingsFactory regionColorSettingsFactory, IColorService colorService)
        {
            _territories = territories;
            _enabledRegions = enabledRegions;
            _selectedRegion = selectedRegion;
            _regionColorSettingsFactory = regionColorSettingsFactory;
            _colorService = colorService;
        }

        public void Visit(RegionViewModel regionViewModel)
        {
            var player = _territories.Single(x => x.Region == regionViewModel.Region).Player;

            var regionColorSettings = _regionColorSettingsFactory.Create(regionViewModel.Region);

            var strokeColor = regionColorSettings.NormalStrokeColor;
            var fillColor = _selectedRegion.Bind(x => x == regionViewModel.Region)
                .Fold(_ => _colorService.SelectedTerritoryColor, () => regionColorSettings.NormalFillColor);
            var mouseOverStrokeColor = regionColorSettings.MouseOverStrokeColor;
            var mouseOverFillColor = regionColorSettings.MouseOverFillColor;

            regionViewModel.StrokeColor = strokeColor;
            regionViewModel.FillColor = fillColor;
            regionViewModel.MouseOverStrokeColor = mouseOverStrokeColor;
            regionViewModel.MouseOverFillColor = mouseOverFillColor;

            regionViewModel.IsEnabled = IsRegionEnabled(regionViewModel);
        }

        private bool IsRegionEnabled(RegionViewModel regionViewModel)
        {
            return _enabledRegions.Contains(regionViewModel.Region);
        }

        public void Visit(RegionNameViewModel regionNameViewModel)
        {
            UpdateArmiesForViewModel(regionNameViewModel);
        }

        private void UpdateArmiesForViewModel(RegionNameViewModel regionNameViewModel)
        {
            var territory = _territories
                .Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = territory.Armies;
        }
    }
}