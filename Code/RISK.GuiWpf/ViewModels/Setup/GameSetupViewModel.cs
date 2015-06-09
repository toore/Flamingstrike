using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainViewModel
    {
        void Activate();
    }

    public class GameSetupViewModel : Screen, ITerritorySelector, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactory _gameFactory;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactory gameFactory,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactory = gameFactory;
            _guiThreadDispatcher = guiThreadDispatcher;
            _taskEx = taskEx;
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { this.NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

        private string _informationText;
        public string InformationText
        {
            get { return _informationText; }
            set { this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        private IPlayer _player;
        public IPlayer Player
        {
            get { return _player; }
            private set { this.NotifyOfPropertyChange(value, () => Player, x => _player = x); }
        }

        public void Activate()
        {
            OnActivate();
        }

        protected override void OnActivate()
        {
            var territorySelector = this;

            _taskEx.Run(() =>
            {
                var game = _gameFactory.Create(territorySelector);

                StartGameplay(game);
            });
        }

        public ITerritory SelectTerritory(ITerritorySelectorParameter territorySelectorParameter)
        {
            _guiThreadDispatcher.Invoke(() => UpdateView(territorySelectorParameter));

            return _userInteractor.GetSelectedTerritory(territorySelectorParameter);
        }

        private void StartGameplay(IGameAdapter gameAdapter)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gameAdapter));
        }

        private void UpdateView(ITerritorySelectorParameter territorySelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(territorySelectorParameter.WorldMap, x => _userInteractor.SelectTerritory(x), territorySelectorParameter.EnabledTerritories);

            WorldMapViewModel = worldMapViewModel;

            Player = territorySelectorParameter.GetPlayerThatTakesTurn();

            InformationText = string.Format(Resources.PLACE_ARMY, territorySelectorParameter.GetArmiesLeft());
        }

        public bool CanFortify()
        {
            return false;
        }

        public void Fortify() { }

        public bool CanEndTurn()
        {
            return false;
        }

        public void EndTurn() { }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }
    }
}