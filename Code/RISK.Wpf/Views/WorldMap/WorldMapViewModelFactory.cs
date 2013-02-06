using System;
using System.Linq;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.TerritoryViewModelFactories;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMap
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelsFactorySelector _territoryViewModelsFactorySelector;
        private readonly ILocationRepository _locationRepository;
        private readonly IColorService _colorService;

        public WorldMapViewModelFactory(ITerritoryViewModelsFactorySelector territoryViewModelsFactorySelector, ILocationRepository locationRepository, IColorService colorService)
        {
            _territoryViewModelsFactorySelector = territoryViewModelsFactorySelector;
            _locationRepository = locationRepository;
            _colorService = colorService;
        }

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ITerritory> selectTerritory)
        {
            var territories = _locationRepository.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            var worldMapViewModels = territories
                .Select(x => CreateTerritoryViewModel(x, selectTerritory))
                .Union(territories.Select(CreateTextViewModel))
                .ToList();

            return new WorldMapViewModel
                {
                    WorldMapViewModels = worldMapViewModels
                };
        }

        private IWorldMapViewModel CreateTerritoryViewModel(ITerritory territory, Action<ITerritory> selectTerritory)
        {
            var territoryViewModel = GetFactory(territory).CreateTerritoryViewModel();
            territoryViewModel.ClickCommand = () => selectTerritory(territory);

            if (territory.HasOwner)
            {
                SetTerritoryOwnerColors(territory, territoryViewModel);
            }

            return territoryViewModel;
        }

        private void SetTerritoryOwnerColors(ITerritory territory, TerritoryViewModel territoryViewModel)
        {
            var playerTerritoryColors = _colorService.GetPlayerTerritoryColors(territory.Owner);

            territoryViewModel.NormalStrokeColor = playerTerritoryColors.NormalStrokeColor;
            territoryViewModel.NormalFillColor = playerTerritoryColors.NormalFillColor;
            territoryViewModel.MouseOverStrokeColor = playerTerritoryColors.MouseOverStrokeColor;
            territoryViewModel.MouseOverFillColor = playerTerritoryColors.MouseOverFillColor;
        }

        private IWorldMapViewModel CreateTextViewModel(ITerritory territory)
        {
            return GetFactory(territory).CreateTextViewModel(territory);
        }

        private ITerritoryViewModelsFactory GetFactory(ITerritory territory)
        {
            return _territoryViewModelsFactorySelector.Select(territory);
        }
    }
}