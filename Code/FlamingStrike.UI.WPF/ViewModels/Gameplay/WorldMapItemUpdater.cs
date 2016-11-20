using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IReadOnlyList<IRegion> _enabledRegions;
        private readonly Maybe<IRegion> _selectedRegion;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public WorldMapItemUpdater(
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IRegion> enabledRegions,
            Maybe<IRegion> selectedRegion,
            IPlayerUiDataRepository playerUiDataRepository)
        {
            _territories = territories;
            _enabledRegions = enabledRegions;
            _selectedRegion = selectedRegion;
            _playerUiDataRepository = playerUiDataRepository;
        }

        public void Visit(RegionViewModel regionViewModel)
        {
            var playerColor = GetPlayerColor(regionViewModel.Region);

            regionViewModel.StrokeColor = playerColor.Darken();
            regionViewModel.FillColor = playerColor;
            regionViewModel.IsSelected = IsSelected(regionViewModel.Region);

            regionViewModel.IsEnabled = IsRegionEnabled(regionViewModel.Region);
        }

        private bool IsSelected(IRegion region)
        {
            return _selectedRegion
                .Fold(x => x == region, () => false);
        }

        private Color GetPlayerColor(IRegion region)
        {
            var player = _territories.Single(x => x.Region == region).Player;
            var playerUiData = _playerUiDataRepository.Get(player);

            return playerUiData.Color;
        }

        private bool IsRegionEnabled(IRegion region)
        {
            return _enabledRegions.Contains(region);
        }

        public void Visit(RegionNameViewModel regionNameViewModel)
        {
            var territory = _territories.Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = territory.Armies;
        }
    }
}