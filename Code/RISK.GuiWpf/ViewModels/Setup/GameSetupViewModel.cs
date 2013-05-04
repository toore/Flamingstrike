using System.Linq;
using Caliburn.Micro;
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
        private readonly IUserInputRequestHandler _userInputRequestHandler;
        private readonly IDispatcherWrapper _dispatcherWrapper;
        private ILocation _selectedLocation;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameFactoryWorker gameFactoryWorker,
            IDispatcherWrapper dispatcherWrapper,
            IGameStateConductor gameStateConductor, 
            IUserInputRequestHandler userInputRequestHandler)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _gameStateConductor = gameStateConductor;
            _userInputRequestHandler = userInputRequestHandler;
            _dispatcherWrapper = dispatcherWrapper;

            _gameFactoryWorker.BeginInvoke(this);
        }

        public ILocation GetLocationCallback(ILocationSelectorParameter locationSelectorParameter)
        {
            _dispatcherWrapper.Invoke(() => UpdateWorldMapViewModel(locationSelectorParameter));

            _userInputRequestHandler.WaitForInput();

            return _selectedLocation;
        }

        private void UpdateWorldMapViewModel(ILocationSelectorParameter locationSelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.WorldMap, SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = locationSelectorParameter.AvailableLocations.Contains(x.Location));

            WorldMapViewModel = worldMapViewModel;
        }

        public void OnFinished(IGame game)
        {
            _dispatcherWrapper.Invoke(() => StartGamePlay(game));
        }

        private void StartGamePlay(IGame game)
        {
            _gameStateConductor.StartGamePlay(game);
        }

        public void SelectLocation(ILocation location)
        {
            _selectedLocation = location;

            _userInputRequestHandler.InputHandled();
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }
    }
}