using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.WorldMapViewModels;
using GuiWpf.Views.WorldMap;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame _game;
        private ITurn _currentTurn;
        private List<ITerritory> _territories;
        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public GameEngine(IGame game, ILocationRepository locationRepository, IWorldMapViewModelFactory worldMapViewModelFactory)
        {
            _game = game;

            var worldMap = _game.GetWorldMap();

            _territories = locationRepository.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            GetNextTurn();

            WorldMapViewModel = worldMapViewModelFactory.Create(worldMap, SelectLocation);
        }

        private void GetNextTurn()
        {
            _currentTurn = _game.GetNextTurn();
        }

        private void SelectLocation(ILocation location)
        {
            _currentTurn.Select(location);

            UpdateWorldMap();
        }

        private void UpdateWorldMap()
        {
            _territories.Apply(UpdateTerritory);
        }

        private void UpdateTerritory(ITerritory territory)
        {
            var territoryViewModel = WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryViewModel>()
                .Single(x => x.Location == territory.Location);

           //territoryViewModel.
        }
    }
}