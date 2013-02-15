using System;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameboard.WorldMap
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

        public WorldMapViewModel Create(IWorldMap worldMap, Action<ILocation> selectLocation)
        {
            var territories = _locationRepository.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            var worldMapViewModels = territories
                .Select(x => CreateTerritoryViewModel(x, selectLocation))
                .Union(territories.Select(CreateTextViewModel))
                .ToList();

            var worldMapViewModel = new WorldMapViewModel();
            worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            return worldMapViewModel;
        }

        private IWorldMapViewModel CreateTerritoryViewModel(ITerritory territory, Action<ILocation> selectLocation)
        {
            return _territoryViewModelFactory.Create(territory, selectLocation);
        }

        private IWorldMapViewModel CreateTextViewModel(ITerritory territory)
        {
            return _textViewModelFactory.Create(territory);
        }
    }
}