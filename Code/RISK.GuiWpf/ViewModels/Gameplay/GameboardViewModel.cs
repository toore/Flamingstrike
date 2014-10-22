using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel
    {
        private readonly IGame _game;
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IGameOverEvaluater _gameOverEvaluater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITurnPhaseFactory _turnPhaseFactory;
        private ITurnState _turnState;
        private readonly List<ITerritory> _territories;
        private IPlayer _player;
        private readonly IWorldMap _worldMap;
        private bool _canFortify = true;
        private ITurnPhase _phase;

        public GameboardViewModel(
            IGame game,
            IEnumerable<ILocation> locations,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IGameOverEvaluater gameOverEvaluater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            ITurnPhaseFactory turnPhaseFactory)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _gameOverEvaluater = gameOverEvaluater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _turnPhaseFactory = turnPhaseFactory;

            _worldMap = _game.GetWorldMap();

            _territories = locations
                .Select(x => _worldMap.GetTerritory(x))
                .ToList();

            InformationText = LanguageResources.Instance.GetString("SELECT_TERRITORY");

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
            _turnState = _game.GetNextTurn();
            _phase = _turnPhaseFactory.CreateAttackPhase(_turnState);
            Player = _turnState.Player;

            UpdateGame();
        }

        public void EndTurn()
        {
            _turnState.EndTurn();

            BeginNextPlayerTurn();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.Publish(new NewGameMessage());
            }
        }

        public void OnLocationClick(ILocation location)
        {
            _phase.OnLocationClick(location);

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
            UpdateTerritoryText(territory);
        }

        private void UpdateTerritoryLayout(ITerritory territory)
        {
            var location = territory.Location;
            var territoryLayout = WorldMapViewModel.WorldMapViewModels.GetTerritoryLayout(location);

            territoryLayout.IsEnabled = CanClick(territory);
            territoryLayout.IsSelected = _turnState.SelectedTerritory == territory;
            _territoryViewModelUpdater.UpdateColors(territoryLayout, territory);
        }

        private bool CanClick(ITerritory territory)
        {
            var location = territory.Location;
            return _turnState.CanAttack(location) || CanSelect(location);
        }

        private bool CanSelect(ILocation location)
        {
            if (_turnState.IsTerritorySelected)
            {
                return _turnState.SelectedTerritory.Location == location;
            }

            return _turnState.CanSelect(location);
        }

        public bool CanFortify()
        {
            return _canFortify;
        }

        public void Fortify()
        {
            _canFortify = false;
            _phase = _turnPhaseFactory.CreateFortifyingPhase(_turnState);
        }

        private void UpdateTerritoryText(ITerritory territory)
        {
            var territoryTextViewModel = WorldMapViewModel.WorldMapViewModels.GetTerritoryTextViewModel(territory.Location);
            territoryTextViewModel.Armies = territory.Armies;
        }
    }

    public interface ITurnPhase
    {
        void OnLocationClick(ILocation location);
    }

    public class AttackPhase : ITurnPhase
    {
        private readonly ITurnState _turnState;

        public AttackPhase(ITurnState turnState)
        {
            _turnState = turnState;
        }

        public void OnLocationClick(ILocation location)
        {
            if (_turnState.CanSelect(location))
            {
                _turnState.Select(location);
            }
            else if (_turnState.CanAttack(location))
            {
                _turnState.Attack(location);
            }
        }
    }

    public class FortifyingPhase : ITurnPhase
    {
        private readonly ITurnState _turnState;

        public FortifyingPhase(ITurnState turnState)
        {
            _turnState = turnState;
        }

        public void OnLocationClick(ILocation location)
        {
            if (_turnState.CanFortify(location))
            {
                _turnState.Fortify(location, 10);
            }
            else
            {

            }
        }
    }

    public interface ITurnPhaseFactory
    {
        AttackPhase CreateAttackPhase(ITurnState turnState);
        FortifyingPhase CreateFortifyingPhase(ITurnState turnState);
    }

    public class TurnPhaseFactory : ITurnPhaseFactory
    {
        public AttackPhase CreateAttackPhase(ITurnState turnState)
        {
            return new AttackPhase(turnState);
        }

        public FortifyingPhase CreateFortifyingPhase(ITurnState turnState)
        {
            return new FortifyingPhase(turnState);
        }
    }
}