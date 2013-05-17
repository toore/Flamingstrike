using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Properties;
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
        private readonly IGameOverEvaluater _gameOverEvaluater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IUserNotifier _userNotifier;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;
        private ITurn _currentTurn;
        private readonly List<ITerritory> _territories;
        private IPlayer _player;
        private readonly IWorldMap _worldMap;

        public GameboardViewModel(
            IGame game,
            ILocationProvider locationProvider,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IGameOverEvaluater gameOverEvaluater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IUserNotifier userNotifier,
            IResourceManagerWrapper resourceManagerWrapper)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _gameOverEvaluater = gameOverEvaluater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _userNotifier = userNotifier;
            _resourceManagerWrapper = resourceManagerWrapper;

            _worldMap = _game.GetWorldMap();

            _territories = locationProvider.GetAll()
                .Select(_worldMap.GetTerritory)
                .ToList();

            InformationText = Resources.SELECT_TERRITORY;

            WorldMapViewModel = worldMapViewModelFactory.Create(_worldMap, OnLocationClick);

            BeginNextPlayerTurn();
        }

        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public IPlayer Player
        {
            get { return _player; }
            set { NotifyOfPropertyChange(value, () => Player, x => _player = x); }
        }

        public string InformationText { get; private set; }

        private void BeginNextPlayerTurn()
        {
            _currentTurn = _game.GetNextTurn();
            Player = _currentTurn.Player;

            UpdateGame();
        }

        public void EndTurn()
        {
            _currentTurn.EndTurn();

            BeginNextPlayerTurn();
        }

        public void EndGame()
        {
            _userNotifier.Confirm(_resourceManagerWrapper.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME"));
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

            UpdateGame();
        }

        private void UpdateGame()
        {
            UpdateWorldMap();

            if (_gameOverEvaluater.IsGameOver(_worldMap))
            {
                _windowManager.ShowDialog(_gameOverViewModelFactory.Create(Player));
            }
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