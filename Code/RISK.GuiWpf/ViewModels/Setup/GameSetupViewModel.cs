using System.Linq;
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

    public class GameSetupViewModel : Screen, ITerritorySelector, IGameInitializerNotifier, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactoryWorker _gameFactoryWorker;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactoryWorker gameFactoryWorker)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactoryWorker = gameFactoryWorker;
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
            throw new System.NotImplementedException();
        }

        protected override void OnActivate()
        {
            _gameFactoryWorker.Run(this, this);
        }

        public ITerritory SelectTerritory(ITerritorySelectorParameter territorySelectorParameter)
        {
            UpdateView(territorySelectorParameter);

            return _userInteractor.GetLocation(territorySelectorParameter);
        }

        public void InitializationFinished(IGame game)
        {
            StartGamePlay(game);
        }

        private void UpdateView(ITerritorySelectorParameter territorySelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(territorySelectorParameter.WorldMap, _userInteractor.SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = territorySelectorParameter.EnabledTerritories.Contains(x.Territory));

            WorldMapViewModel = worldMapViewModel;

            Player = territorySelectorParameter.GetPlayerThatTakesTurn();

            InformationText = string.Format(Resources.PLACE_ARMY, territorySelectorParameter.GetArmiesLeft());
        }

        private void StartGamePlay(IGame game)
        {
            _eventAggregator.PublishOnUIThread(new StartGameMessage(game));
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