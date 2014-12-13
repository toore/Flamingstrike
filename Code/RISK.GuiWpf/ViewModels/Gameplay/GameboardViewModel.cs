using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel
    {
        private readonly IGame _game;
        private readonly ITerritoryViewModelColorInitializer _territoryViewModelColorInitializer;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private IPlayer _player;

        public GameboardViewModel(
            IGame game,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelColorInitializer territoryViewModelColorInitializer,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _game = game;
            _territoryViewModelColorInitializer = territoryViewModelColorInitializer;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            InformationText = LanguageResources.Instance.GetString("SELECT_TERRITORY");

            WorldMapViewModel = worldMapViewModelFactory.Create(_game.WorldMap, OnTerritoryClick);

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
            Player = _game.CurrentInteractionState.Player;

            UpdateGameBoard();
        }

        public void Fortify()
        {
            _game.Fortify();
        }

        public void EndTurn()
        {
            _game.EndTurn();

            BeginNextPlayerTurn();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnCurrentThread(new NewGameMessage());
            }
        }

        public void OnTerritoryClick(ITerritory territory)
        {
            _game.CurrentInteractionState.OnClick(territory);

            UpdateGameBoard();
        }

        private void UpdateGameBoard()
        {
            if (_game.IsGameOver())
            {
                _windowManager.ShowDialog(_gameOverViewModelFactory.Create(Player));
            }
            else
            {
                UpdateWorldMap();
            }
        }

        private void UpdateWorldMap()
        {
            foreach (var territory in _game.WorldMap.GetTerritories())
            {
                UpdateTerritory(territory);
            }
        }

        private void UpdateTerritory(ITerritory territory)
        {
            UpdateTerritoryLayout(territory);
            UpdateTerritoryText(territory);
        }

        private void UpdateTerritoryLayout(ITerritory territory)
        {
            var layoutViewModel = WorldMapViewModel.WorldMapViewModels
                .OfType<TerritoryLayoutViewModel>()
                .Single(x => x.Territory == territory);

            var interactionState = _game.CurrentInteractionState;
            layoutViewModel.IsEnabled = interactionState.CanClick(territory);
            layoutViewModel.IsSelected = interactionState.SelectedTerritory == territory;

            _territoryViewModelColorInitializer.UpdateColors(_game.WorldMap, layoutViewModel);
        }

        private void UpdateTerritoryText(ITerritory territory)
        {
            var textViewModel = WorldMapViewModel.WorldMapViewModels
                .OfType<TerritoryTextViewModel>()
                .Single(x => x.Territory == territory);

            textViewModel.Armies = territory.Armies;
        }
    }
}