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
        private ITurn _turn;
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
            _turn = _game.GetNextTurn();
            _phase = _turnPhaseFactory.CreateAttackPhase(_turn);
            Player = _turn.Player;

            UpdateGame();
        }

        public void EndTurn()
        {
            _turn.EndTurn();

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
            territoryLayout.IsSelected = _turn.SelectedTerritory == territory;
            _territoryViewModelUpdater.UpdateColors(territoryLayout, territory);
        }

        private bool CanClick(ITerritory territory)
        {
            var location = territory.Location;
            return _turn.CanAttack(location) || CanSelect(location);
        }

        private bool CanSelect(ILocation location)
        {
            if (_turn.IsTerritorySelected)
            {
                return _turn.SelectedTerritory.Location == location;
            }

            return _turn.CanSelect(location);
        }

        public bool CanFortify()
        {
            return _canFortify;
        }

        public void Fortify()
        {
            _canFortify = false;
            _phase = _turnPhaseFactory.CreateFortifyingPhase(_turn);
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
        private readonly ITurn _turn;

        public AttackPhase(ITurn turn)
        {
            _turn = turn;
        }

        public void OnLocationClick(ILocation location)
        {
            if (_turn.CanSelect(location))
            {
                _turn.Select(location);
            }
            else if (_turn.CanAttack(location))
            {
                _turn.Attack(location);
            }
        }
    }

    public class FortifyingPhase : ITurnPhase
    {
        private readonly ITurn _turn;

        public FortifyingPhase(ITurn turn)
        {
            _turn = turn;
        }

        public void OnLocationClick(ILocation location)
        {
            if (_turn.CanFortify(location))
            {
                _turn.Fortify(location, 10);
            }
            else
            {

            }
        }
    }

    public interface ITurnPhaseFactory
    {
        AttackPhase CreateAttackPhase(ITurn turn);
        FortifyingPhase CreateFortifyingPhase(ITurn turn);
    }

    public class TurnPhaseFactory : ITurnPhaseFactory
    {
        public AttackPhase CreateAttackPhase(ITurn turn)
        {
            return new AttackPhase(turn);
        }

        public FortifyingPhase CreateFortifyingPhase(ITurn turn)
        {
            return new FortifyingPhase(turn);
        }
    }
}