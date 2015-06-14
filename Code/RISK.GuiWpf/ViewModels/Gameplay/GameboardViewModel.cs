using System;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel, IActivate
    {
        private readonly IGameAdapter _gameAdapter;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private IPlayerId _playerId;

        public GameboardViewModel(
            IGameAdapter gameAdapter,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _gameAdapter = gameAdapter;
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
            //WorldMapViewModel = _worldMapViewModelFactory.Create(_gameAdapter.WorldMap, x => OnTerritoryClick(x), Enumerable.Empty<ITerritory>());

            UpdateGame();
        }

        public void Fortify()
        {
            _gameAdapter.Fortify();
        }

        public void EndTurn()
        {
            _gameAdapter.EndTurn();

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

        public void OnTerritoryClick(ITerritory territory)
        {
            _gameAdapter.OnClick(territory);

            UpdateGame();
        }

        private void UpdateGame()
        {
            PlayerId = _gameAdapter.PlayerId;

            if (_gameAdapter.IsGameOver())
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
            var enabledTerritories = _gameAdapter.WorldMap.GetTerritories()
                .Where(x => _gameAdapter.CanClick(x))
                .ToList();

            _worldMapViewModelFactory.Update(WorldMapViewModel, _gameAdapter.WorldMap, _gameAdapter.SelectedTerritory, enabledTerritories);
        }
    }
}