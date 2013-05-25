using System.Linq;
using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IUserInteractionSynchronizer _userInteractionSynchronizer;
        private readonly IDialogManager _dialogManager;
        private ILocation _selectedLocation;
        private ILocationSelectorParameter _locationSelectorParameter;
        private bool _isGameSetupFinished;
        private IGame _game;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameFactoryWorker gameFactoryWorker,
            IGameStateConductor gameStateConductor,
            IUserInteractionSynchronizer userInteractionSynchronizer,
            IDialogManager dialogManager)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _gameStateConductor = gameStateConductor;
            _userInteractionSynchronizer = userInteractionSynchronizer;
            _dialogManager = dialogManager;

            _gameFactoryWorker.BeginInvoke(this);

            WaitForUserInputRequestAndUpdateView();
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

        private void WaitForUserInputRequestAndUpdateView()
        {
            _userInteractionSynchronizer.WaitForUserInteractionRequest();

            UpdateView(_locationSelectorParameter);
        }

        public ILocation GetLocationCallback(ILocationSelectorParameter locationSelectorParameter)
        {
            _locationSelectorParameter = locationSelectorParameter;

            _userInteractionSynchronizer.RequestUserInteraction();
            _userInteractionSynchronizer.WaitForUserToBeDoneWithInteracting();

            return _selectedLocation;
        }

        public void SelectLocation(ILocation location)
        {
            _selectedLocation = location;

            _userInteractionSynchronizer.UserIsDoneInteracting();
            WaitForUserInputRequestAndUpdateView();

            if (_isGameSetupFinished)
            {
                StartGamePlay(_game);
            }
        }

        public void OnFinished(IGame game)
        {
            _isGameSetupFinished = true;
            _game = game;

            _userInteractionSynchronizer.RequestUserInteraction();
        }

        private void UpdateView(ILocationSelectorParameter locationSelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.WorldMap, SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = locationSelectorParameter.AvailableLocations.Contains(x.Location));

            WorldMapViewModel = worldMapViewModel;

            Player = _locationSelectorParameter.PlayerDuringSetup.GetInGamePlayer();

            InformationText = string.Format(Resources.PLACE_ARMY, locationSelectorParameter.PlayerDuringSetup.Armies);
        }

        private void StartGamePlay(IGame game)
        {
            _gameStateConductor.StartGamePlay(game);
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
        }
    }
}