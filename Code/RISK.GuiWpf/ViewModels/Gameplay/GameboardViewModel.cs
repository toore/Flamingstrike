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
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private IPlayer _player;

        public GameboardViewModel(
            IGame game,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _game = game;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            InformationText = LanguageResources.Instance.GetString("SELECT_TERRITORY");

            WorldMapViewModel = _worldMapViewModelFactory.Create(_game.WorldMap, x => OnTerritoryClick(x), Enumerable.Empty<ITerritory>());

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
            Player = _game.Player;

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
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        public void OnTerritoryClick(ITerritory territory)
        {
            _game.OnClick(territory);

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
            var enabledTerritories = _game.WorldMap.GetTerritories()
                .Where(x => _game.CanClick(x))
                .ToList();

            _worldMapViewModelFactory.Update(WorldMapViewModel, _game.WorldMap, _game.SelectedTerritory, enabledTerritories);
        }
    }
}