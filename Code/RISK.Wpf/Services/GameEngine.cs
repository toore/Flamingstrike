using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay.WorldMap;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Services
{
    public class GameEngine : IGameEngine
    {
        private readonly IGame _game;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITurn _currentTurn;
        private readonly List<ITerritory> _territories;
        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public GameEngine(IGame game, ILocationProvider locationProvider, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;

            var worldMap = _game.GetWorldMap();

            _territories = locationProvider.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            BeginNextPlayerTurn();

            WorldMapViewModel = worldMapViewModelFactory.Create(worldMap, SelectLocation);
        }

        private void BeginNextPlayerTurn()
        {
            _currentTurn = _game.GetNextTurn();
        }

        public void SelectLocation(ILocation location)
        {
            if (_currentTurn.IsTerritorySelected)
            {
                _currentTurn.Attack(location);
            }
            else
            {
                _currentTurn.Select(location);
            }

            UpdateWorldMap();
        }

        private void UpdateWorldMap()
        {
            _territories.Apply(UpdateTerritory);
        }

        private void UpdateTerritory(ITerritory territory)
        {
            var location = territory.Location;
            var territoryViewModel = FindTerritoryViewModel(location, WorldMapViewModel.WorldMapViewModels);

            territoryViewModel.IsEnabled = _currentTurn.CanSelect(location);
            _territoryViewModelUpdater.UpdateColor(territoryViewModel, territory);
        }

        private static ITerritoryViewModel FindTerritoryViewModel(ILocation location, IEnumerable<IWorldMapViewModel> worldMapViewModels)
        {
            return worldMapViewModels
                .OfType<ITerritoryViewModel>()
                .Single(x => x.Location == location);
        }
    }
}