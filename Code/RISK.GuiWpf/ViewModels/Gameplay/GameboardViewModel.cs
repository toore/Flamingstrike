using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel, IActivate
    {
        private readonly IRegions _regions;
        private readonly IGame _game;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private string _playerName;
        private IInteractionStateFsm _interactionStateFsm;

        public GameboardViewModel(
            IGame game,
            IStateControllerFactory stateControllerFactory,
            IInteractionStateFactory interactionStateFactory,
            IRegions regions,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _regions = regions;
            _game = game;
            _stateControllerFactory = stateControllerFactory;
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            InformationText = ResourceManager.Instance.GetString("SELECT_TERRITORY");
        }

        public WorldMapViewModel WorldMapViewModel { get; set; }

        public string PlayerName
        {
            get { return _playerName; }
            set { NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
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
            var allTerritories = GetAllTerritories();
            WorldMapViewModel = _worldMapViewModelFactory.Create(allTerritories, OnRegionClick, Enumerable.Empty<IRegion>());

            _interactionStateFsm = _stateControllerFactory.Create(_game);
            var draftArmiesState = _interactionStateFactory.CreateDraftArmiesState();
            _interactionStateFsm.Set(draftArmiesState);

            UpdateGame();
        }

        private ReadOnlyCollection<ITerritory> GetAllTerritories()
        {
            var territories = _regions.GetAll()
                .Select(region => _game.GetTerritory(region))
                .ToList().AsReadOnly();

            return territories;
        }

        public bool CanFortify()
        {
            return false; //_game.CanFortify();??
        }

        public void Fortify()
        {
            var fortifySelectState = _interactionStateFactory.CreateFortifySelectState();
            _interactionStateFsm.Set(fortifySelectState);
        }

        public bool CanEndTurn()
        {
            return true;
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

        public void OnRegionClick(IRegion region)
        {
            _interactionStateFsm.OnClick(region);

            UpdateGame();

            if (_game.IsGameOver())
            {
                ShowGameOverMessage();
            }
        }

        private void ShowGameOverMessage()
        {
            var gameOverViewModel = _gameOverViewModelFactory.Create(_game.CurrentPlayer.Name);
            _windowManager.ShowDialog(gameOverViewModel);
        }

        private void UpdateGame()
        {
            PlayerName = _game.CurrentPlayer.Name;

            UpdateWorldMap();
        }

        private void UpdateWorldMap()
        {
            var allTerritories = GetAllTerritories();
            var enabledTerritories = _regions.GetAll()
                .Where(x => _interactionStateFsm.CanClick(x))
                .ToList();

            _worldMapViewModelFactory.Update(WorldMapViewModel, allTerritories, _interactionStateFsm.SelectedRegion, enabledTerritories);
        }
    }
}