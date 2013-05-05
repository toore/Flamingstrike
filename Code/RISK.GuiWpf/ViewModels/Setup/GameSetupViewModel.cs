using System.Linq;
using Caliburn.Micro;
using GuiWpf.Properties;
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
        private readonly IInputRequestHandler _inputRequestHandler;
        private ILocation _selectedLocation;
        private ILocationSelectorParameter _locationSelectorParameter;
        private bool _isGameSetupFinished;
        private IGame _game;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameFactoryWorker gameFactoryWorker,
            IGameStateConductor gameStateConductor,
            IInputRequestHandler inputRequestHandler)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _gameStateConductor = gameStateConductor;
            _inputRequestHandler = inputRequestHandler;

            _gameFactoryWorker.BeginInvoke(this);

            WaitForUserInputRequestAndUpdateWorldMap();
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

        public string InformationText
        {
            get { return Resources.PLACE_ARMY; }
        }

        private void WaitForUserInputRequestAndUpdateWorldMap()
        {
            _inputRequestHandler.WaitForInputRequest();

            UpdateWorldMapViewModel(_locationSelectorParameter);
        }

        public ILocation GetLocationCallback(ILocationSelectorParameter locationSelectorParameter)
        {
            _locationSelectorParameter = locationSelectorParameter;

            _inputRequestHandler.RequestInput();
            _inputRequestHandler.WaitForInputAvailable();

            return _selectedLocation;
        }

        public void SelectLocation(ILocation location)
        {
            _selectedLocation = location;

            _inputRequestHandler.InputIsAvailable();
            WaitForUserInputRequestAndUpdateWorldMap();

            if (_isGameSetupFinished)
            {
                StartGamePlay(_game);
            }
        }

        public void OnFinished(IGame game)
        {
            _isGameSetupFinished = true;
            _game = game;

            _inputRequestHandler.RequestInput();
        }

        private void UpdateWorldMapViewModel(ILocationSelectorParameter locationSelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.WorldMap, SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = locationSelectorParameter.AvailableLocations.Contains(x.Location));

            WorldMapViewModel = worldMapViewModel;
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
    }
}