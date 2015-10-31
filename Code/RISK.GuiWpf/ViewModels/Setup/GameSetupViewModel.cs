using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application.Play;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainViewModel {}

    public class GameSetupViewModel : Screen, ITerritoryRequestHandler, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModel(
            IGameFactory gameFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IAlternateGameSetup alternateGameSetup,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactory = gameFactory;
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

        private string _playerName;
        public string PlayerName
        {
            get { return _playerName; }
            private set { this.NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
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
                var gameSetup = _alternateGameSetup.Initialize(territoryRequestHandler);
                var game = _gameFactory.Create(gameSetup);

                StartGameplay(game);
            });
        }

        public ITerritoryId ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            _guiThreadDispatcher.Invoke(() => UpdateView(territoryRequestParameter));

            return _userInteractor.GetSelectedTerritory(territoryRequestParameter);
        }

        private void StartGameplay(IGame game)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(game));
        }

        private void UpdateView(ITerritoryRequestParameter territoryRequestParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(
                territoryRequestParameter.GameboardTerritories,
                x => _userInteractor.SelectTerritory(x),
                territoryRequestParameter.EnabledTerritories);

            WorldMapViewModel = worldMapViewModel;

            PlayerName = territoryRequestParameter.PlayerId.Name;

            InformationText = string.Format(Resources.PLACE_ARMY, territoryRequestParameter.GetArmiesLeftToPlace());
        }

        public bool CanFortify()
        {
            return false;
        }

        public void Fortify() {}

        public bool CanEndTurn()
        {
            return false;
        }

        public void EndTurn() {}

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