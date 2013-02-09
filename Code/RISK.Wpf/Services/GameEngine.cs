using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.WorldMapViewModels;
using GuiWpf.Views.WorldMapViews;
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

        public GameEngine(IGame game, ILocationRepository locationRepository, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;

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

        public void SelectLocation(ILocation location)
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
            var location = territory.Location;

            var territoryViewModel = WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryViewModel>()
                .Single(x => x.Location == location);

            territoryViewModel.IsEnabled = _currentTurn.CanSelect(location);
            
            _territoryViewModelUpdater.UpdateColor(territoryViewModel, territory);
        }
    }
}