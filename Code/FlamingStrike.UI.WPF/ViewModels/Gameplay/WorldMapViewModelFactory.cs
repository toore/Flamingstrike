using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class Territory
    {
        public Territory(Region region, bool isEnabled, PlayerName player, int armies)
        {
            Region = region;
            IsEnabled = isEnabled;
            Player = player;
            Armies = armies;
        }

        public Region Region { get; }
        public bool IsEnabled { get; }
        public PlayerName Player { get; }
        public int Armies { get; }
    }

    public interface IWorldMapViewModelFactory
    {
        WorldMapViewModel Create(Action<Region> onClick);

        void Update(WorldMapViewModel worldMapViewModel, IReadOnlyList<Territory> territories, Maybe<Region> selectedRegion);
    }

    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly RegionModelFactory _regionModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public WorldMapViewModelFactory(
            RegionModelFactory regionModelFactory,
            IPlayerUiDataRepository playerUiDataRepository)
        {
            _regionModelFactory = regionModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
        }

        public WorldMapViewModel Create(Action<Region> onClick)
        {
            var worldMapViewModel = new WorldMapViewModel();
            var worldMapItems = CreateWorldMapItemViewModels(onClick);
            foreach (var viewModel in worldMapItems)
            {
                worldMapViewModel.WorldMapViewModels.Add(viewModel);
            }

            return worldMapViewModel;
        }

        public void Update(
            WorldMapViewModel worldMapViewModel,
            IReadOnlyList<Territory> territories,
            Maybe<Region> selectedRegion)
        {
            var worldMapItemUpdater = new WorldMapItemUpdater(
                territories,
                selectedRegion,
                _playerUiDataRepository);

            foreach (var worldMapItemViewModel in worldMapViewModel.WorldMapViewModels)
            {
                worldMapItemViewModel.Accept(worldMapItemUpdater);
            }
        }

        private IEnumerable<IWorldMapItemViewModel> CreateWorldMapItemViewModels(Action<Region> onClick)
        {
            var regionModels = _regionModelFactory.Create().ToList();
            IEnumerable<IWorldMapItemViewModel> regionViewModels = regionModels.Select(regionModel => new RegionViewModel(regionModel, onClick));
            IEnumerable<IWorldMapItemViewModel> titleViewModels = regionModels.Select(regionModel => new RegionNameViewModel(regionModel));

            var worldMapViewModels = regionViewModels
                .Concat(titleViewModels)
                .ToList();

            return worldMapViewModels;
        }
    }
}