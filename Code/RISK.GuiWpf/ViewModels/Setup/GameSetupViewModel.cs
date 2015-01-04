using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
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

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactory gameFactory,
            IGuiThreadDispatcher guiThreadDispatcher)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactory = gameFactory;
            _guiThreadDispatcher = guiThreadDispatcher;
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

            Task.Run(() =>
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

        private void StartGameplay(IGame game)
        {
            _eventAggregator.PublishOnCurrentThread(new StartGameplayMessage(game));
        }

        private void UpdateView(ITerritorySelectorParameter territorySelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(territorySelectorParameter.WorldMap, _userInteractor.SelectTerritory);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = territorySelectorParameter.EnabledTerritories.Contains(x.Territory));

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
                _eventAggregator.PublishOnCurrentThread(new NewGameMessage());
            }
        }
    }
}