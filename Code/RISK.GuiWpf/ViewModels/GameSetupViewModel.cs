using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IGameStateConductor _gameStateConductor;
        private ILocation _selectedLocation;

        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public GameSetupViewModel(IWorldMapViewModelFactory worldMapViewModelFactory, IGameFactory gameFactory, IGameStateConductor gameStateConductor)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactory = gameFactory;
            _gameStateConductor = gameStateConductor;

            Task.Run(() =>
                {
                    var game = _gameFactory.Create(this);
                    _gameStateConductor.StartGamePlay(game);
                });
        }

        public ILocation GetLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.WorldMap, SelectLocation);
                    worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                        .Apply(x => x.IsEnabled = locationSelectorParameter.AvailableLocations.Contains(x.Location));
                    WorldMapViewModel = worldMapViewModel;
                });

            _autoResetEvent.WaitOne();

            return _selectedLocation;
        }

        private void SelectLocation(ILocation location)
        {
            _selectedLocation = location;

            _autoResetEvent.Set();
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }
    }
}