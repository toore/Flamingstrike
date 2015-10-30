using System;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel, IActivate
    {
        private readonly IWorldMap _worldMap;
        private readonly IGame _game;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private IPlayerId _playerId;
        private IStateController _stateController;

        public GameboardViewModel(
            IGame game,
            IStateControllerFactory stateControllerFactory,
            IInteractionStateFactory interactionStateFactory,
            IWorldMap worldMap,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMap = worldMap;
            _game = game;
            _stateControllerFactory = stateControllerFactory;
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            InformationText = LanguageResources.Instance.GetString("SELECT_TERRITORY");
        }

        public WorldMapViewModel WorldMapViewModel { get; set; }

        public IPlayerId PlayerId
        {
            get { return _playerId; }
            set { NotifyOfPropertyChange(value, () => PlayerId, x => _playerId = x); }
        }

        public string InformationText { get; private set; }

        public void Activate()
        {
            InitializeWorld();
        }

        public bool IsActive { get; private set; }
        public event EventHandler<ActivationEventArgs> Activated;

        private void InitializeWorld()
        {
            WorldMapViewModel = _worldMapViewModelFactory.Create(_game.Territories, OnTerritoryClick, Enumerable.Empty<ITerritoryId>());

            _stateController = _stateControllerFactory.Create(_game);
            _stateController.CurrentState = _interactionStateFactory.CreateSelectState();

            UpdateGame();
        }

        public void Fortify()
        {
            //_interactionState = _interactionStateFactory.CreateFortifyState(_stateController);
        }

        public void EndTurn()
        {
            _game.EndTurn();

            UpdateGame();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        public void OnTerritoryClick(ITerritoryId territoryId)
        {
            _stateController.OnClick(territoryId);

            UpdateGame();
        }

        private void UpdateGame()
        {
            PlayerId = _game.CurrentPlayer.PlayerId;

            if (_game.IsGameOver())
            {
                _windowManager.ShowDialog(_gameOverViewModelFactory.Create(PlayerId));
            }
            else
            {
                UpdateWorldMap();
            }
        }

        private void UpdateWorldMap()
        {
            var enabledTerritories = _worldMap.GetAll()
                .Where(x => _stateController.CanClick(x))
                .ToList();

            _worldMapViewModelFactory.Update(WorldMapViewModel, _game.Territories, _stateController.SelectedTerritoryId, enabledTerritories);
        }
    }
}