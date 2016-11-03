using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public class WorldMapItemUpdater : IWorldMapItemViewModelVisitor
    {
        private readonly IReadOnlyList<ITerritory> _territories;
        private readonly IReadOnlyList<IRegion> _enabledRegions;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public WorldMapItemUpdater(
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IRegion> enabledRegions,
            IPlayerUiDataRepository playerUiDataRepository)
        {
            _territories = territories;
            _enabledRegions = enabledRegions;
            _playerUiDataRepository = playerUiDataRepository;
        }

        public void Visit(RegionViewModel regionViewModel)
        {
            var player = _territories.Single(x => x.Region == regionViewModel.Region).Player;
            var playerUiData = _playerUiDataRepository.Get(player);

            var strokeColor = Darken(playerUiData.Color);
            var fillColor = playerUiData.Color;

            regionViewModel.StrokeColor = strokeColor;
            regionViewModel.FillColor = fillColor;

            regionViewModel.IsEnabled = IsRegionEnabled(regionViewModel);
        }

        private static Color Darken(Color color)
        {
            var darkerColor = Color.Multiply(color, 0.5f);
            darkerColor.A = 0xff;

            return darkerColor;
        }

        private bool IsRegionEnabled(RegionViewModel regionViewModel)
        {
            return _enabledRegions.Contains(regionViewModel.Region);
        }

        public void Visit(RegionNameViewModel regionNameViewModel)
        {
            var territory = _territories.Single(x => x.Region == regionNameViewModel.Region);

            regionNameViewModel.Armies = territory.Armies;
        }
    }
}