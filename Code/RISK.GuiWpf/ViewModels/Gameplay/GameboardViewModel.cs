using System.Collections.Generic;
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
        private readonly ITerritoryViewModelUpdater _territoryViewModelUpdater;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IEnumerable<ITerritory> _territories;
        private IPlayer _player;

        public GameboardViewModel(
            IGame game,
            IEnumerable<ITerritory> territories,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            ITerritoryViewModelUpdater territoryViewModelUpdater,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _game = game;
            _territoryViewModelUpdater = territoryViewModelUpdater;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            _territories = territories;

            InformationText = LanguageResources.Instance.GetString("SELECT_TERRITORY");

            WorldMapViewModel = worldMapViewModelFactory.Create(_game.Territories.GetAll(), OnLocationClick);

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
                _eventAggregator.Publish(new NewGameMessage());
            }
        }

        public void OnLocationClick(ITerritory location)
        {
            _game.CurrentInteractionState.OnClick(location);

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
            _territories.Apply(UpdateTerritory);
        }

        private void UpdateTerritory(ITerritory territory)
        {
            UpdateTerritoryLayout(territory);
            UpdateTerritoryText(territory);
        }

        private void UpdateTerritoryLayout(ITerritory territory)
        {
            var territoryLayout = WorldMapViewModel.WorldMapViewModels.GetTerritoryLayout(territory);

            var interactionState = _game.CurrentInteractionState;
            territoryLayout.IsEnabled = interactionState.CanClick(territory);
            territoryLayout.IsSelected = interactionState.SelectedTerritory == territory;
            _territoryViewModelUpdater.UpdateColors(territoryLayout, territory);
        }

        private void UpdateTerritoryText(ITerritory territory)
        {
            var territoryTextViewModel = WorldMapViewModel.WorldMapViewModels.GetTerritoryTextViewModel(territory);
            territoryTextViewModel.Armies = territory.Armies;
        }
    }
}