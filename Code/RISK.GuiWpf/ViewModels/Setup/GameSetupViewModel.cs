using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.GameSetup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainViewModel
    {
        void Activate();
    }

    public class GameSetupViewModel : Screen, ITerritoryRequestHandler, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IAlternateGameSetup alternateGameSetup,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _alternateGameSetup = alternateGameSetup;
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
            private set { this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        private IPlayerId _playerId;
        public IPlayerId PlayerId
        {
            get { return _playerId; }
            private set { this.NotifyOfPropertyChange(value, () => PlayerId, x => _playerId = x); }
        }

        public void Activate()
        {
            OnActivate();
        }

        protected override void OnActivate()
        {
            var territoryRequestHandler = this;

            _taskEx.Run(() =>
            {
                var gameboard = _alternateGameSetup.Initialize(territoryRequestHandler);

                //var game = _gameFactory.Create(territorySelector);

                //StartGameplay(game);
            });
        }

        public ITerritory ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            _guiThreadDispatcher.Invoke(() => UpdateView(territoryRequestParameter));

            return _userInteractor.GetSelectedTerritory(territoryRequestParameter);
        }

        private void StartGameplay(IGameAdapter gameAdapter)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gameAdapter));
        }

        private void UpdateView(ITerritoryRequestParameter territoryRequestParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(territoryRequestParameter.Gameboard, x => _userInteractor.SelectTerritory(x), territoryRequestParameter.EnabledTerritories);

            WorldMapViewModel = worldMapViewModel;

            PlayerId = territoryRequestParameter.GetPlayerThatTakesTurn();

            InformationText = string.Format(Resources.PLACE_ARMY, territoryRequestParameter.GetArmiesLeftToPlace());
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