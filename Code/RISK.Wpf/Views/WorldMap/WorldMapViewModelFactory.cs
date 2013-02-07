using System;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.TerritoryViewModelFactories;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Action = System.Action;

namespace GuiWpf.Views.WorldMap
{
    public class WorldMapViewModelFactory : IWorldMapViewModelFactory
    {
        private readonly ITerritoryViewModelFactory _territoryViewModelFactory;
        private readonly ITextViewModelFactory _textViewModelFactory;
        private readonly ILocationRepository _locationRepository;

        public WorldMapViewModelFactory(ILocationRepository locationRepository, ITerritoryViewModelFactory territoryViewModelFactory, ITextViewModelFactory textViewModelFactory)
        {
            _territoryViewModelFactory = territoryViewModelFactory;
            _textViewModelFactory = textViewModelFactory;
            _locationRepository = locationRepository;
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

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            return worldMapViewModel;
        }

        private IWorldMapViewModel CreateTerritoryViewModel(ITerritory territory, Action<ITerritory> selectTerritory)
        {
            Action action = () => selectTerritory(territory);

            return _territoryViewModelFactory.Create(territory, action);
        }

        private IWorldMapViewModel CreateTextViewModel(ITerritory territory)
        {
            return _textViewModelFactory.Create(territory);
        }
    }
}