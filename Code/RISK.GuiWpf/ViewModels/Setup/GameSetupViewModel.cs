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
        void StartSetup();
    }

    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel, IGameInitializerLocationSelector, IGameInitializerNotifier
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameSettingStateConductor _gameSettingStateConductor;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactoryWorker _gameFactoryWorker;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameSettingStateConductor gameSettingStateConductor,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactoryWorker gameFactoryWorker)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameSettingStateConductor = gameSettingStateConductor;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactoryWorker = gameFactoryWorker;
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

        private string _informationText;
        public string InformationText
        {
            get { return _informationText; }
            set { NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        private IPlayer _player;
        public IPlayer Player
        {
            get { return _player; }
            private set { NotifyOfPropertyChange(value, () => Player, x => _player = x); }
        }

        public void StartSetup()
        {
            _gameFactoryWorker.Run(this, this);
        }

        public ITerritory SelectLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            UpdateView(locationSelectorParameter);

            return _userInteractor.GetLocation(locationSelectorParameter);
        }

        public void InitializationFinished(IGame game)
        {
            StartGamePlay(game);
        }

        private void UpdateView(ILocationSelectorParameter locationSelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.AllTerritories, _userInteractor.SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = locationSelectorParameter.EnabledTerritories.Contains(x.Location));

            WorldMapViewModel = worldMapViewModel;

            Player = locationSelectorParameter.GetPlayerThatTakesTurn();

            InformationText = string.Format(Resources.PLACE_ARMY, locationSelectorParameter.GetArmiesLeft());
        }

        private void StartGamePlay(IGame game)
        {
            _gameSettingStateConductor.StartGamePlay(game);
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
                _eventAggregator.Publish(new NewGameMessage());
            }
        }
    }
}