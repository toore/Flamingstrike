using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<Territory> _territories;
        private readonly Maybe<Region> _selectedRegion;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public WorldMapItemUpdater(
            IReadOnlyList<Territory> territories,
            Maybe<Region> selectedRegion,
            IPlayerUiDataRepository playerUiDataRepository)
        {
            _territories = territories;
            _selectedRegion = selectedRegion;
            _playerUiDataRepository = playerUiDataRepository;
        }

        public void Visit(RegionViewModel regionViewModel)
        {
            var playerColor = GetPlayerColor(regionViewModel.Region);

            regionViewModel.StrokeColor = playerColor.Darken();
            regionViewModel.FillColor = playerColor;
            regionViewModel.IsSelected = IsSelected(regionViewModel.Region);

            regionViewModel.IsEnabled = IsEnabled(regionViewModel.Region);
        }

        private bool IsSelected(Region region)
        {
            return _selectedRegion
                .Fold(x => x == region, () => false);
        }

        private bool IsEnabled(Region region)
        {
            return _territories.Single(x => x.Region == region).IsEnabled;
        }

        private Color GetPlayerColor(Region region)
        {
            var player = _territories.Single(x => x.Region == region).Player;
            var playerUiData = _playerUiDataRepository.Get((string)player);

            return playerUiData.Color;
        }

        public void Visit(RegionNameViewModel regionNameViewModel)
        {
            var territory = _territories.Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = territory.Armies;
        }
    }
}