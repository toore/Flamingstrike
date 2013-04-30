using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel
    {
        private readonly IGame _game;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private ITurn _currentTurn;
        private readonly List<ITerritory> _territories;
        private IPlayer _player;
        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public GameboardViewModel(IGame game, ILocationProvider locationProvider, IWorldMapViewModelFactory worldMapViewModelFactory, ITerritoryViewModelUpdater territoryViewModelUpdater)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;

            var worldMap = _game.GetWorldMap();

            _territories = locationProvider.GetAll()
                .Select(worldMap.GetTerritory)
                .ToList();

            WorldMapViewModel = worldMapViewModelFactory.Create(worldMap, OnLocationClick);

            BeginNextPlayerTurn();
        }

        public IPlayer Player
        {
            get { return _player; }
            set { NotifyOfPropertyChange(value, () => Player, x => _player = x); }
        }

        private void BeginNextPlayerTurn()
        {
            _currentTurn = _game.GetNextTurn();
            Player = _currentTurn.Player;

            UpdateWorldMap();
        }

        public void EndTurn()
        {
            _currentTurn.EndTurn();

            BeginNextPlayerTurn();
        }

        public void OnLocationClick(ILocation location)
        {
            if (_currentTurn.CanSelect(location))
            {
                _currentTurn.Select(location);
            }
            else if (_currentTurn.CanAttack(location))
            {
                _currentTurn.Attack(location);
            }

            UpdateWorldMap();
        }

        private void UpdateWorldMap()
        {
            _territories.Apply(UpdateTerritory);
        }

        private void UpdateTerritory(ITerritory territory)
        {
            UpdateTerritoryLayout(territory);

            UpdateTerritoryData(territory);
        }

        private void UpdateTerritoryLayout(ITerritory territory)
        {
            var location = territory.Location;
            var territoryLayout = WorldMapViewModel.WorldMapViewModels.GetTerritoryLayout(location);

            territoryLayout.IsEnabled = CanClick(territory);
            territoryLayout.IsSelected = _currentTurn.SelectedTerritory == territory;
            _territoryViewModelUpdater.UpdateColors(territoryLayout, territory);
        }

        private bool CanClick(ITerritory territory)
        {
            var location = territory.Location;
            return _currentTurn.CanAttack(location) || CanSelect(location);
        }

        private bool CanSelect(ILocation location)
        {
            if (_currentTurn.IsTerritorySelected)
            {
                return _currentTurn.SelectedTerritory.Location == location;
            }

            return _currentTurn.CanSelect(location);
        }

        private void UpdateTerritoryData(ITerritory territory)
        {
            var territoryData = WorldMapViewModel.WorldMapViewModels.GetTerritoryData(territory.Location);
            territoryData.Armies = territory.Armies;
        }
    }
}