using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModel : IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private ILocation _selectedLocation;

        public GameSetupViewModel(/*IWorldMapViewModelFactory worldMapViewModelFactory*/)
        {
            //_worldMapViewModelFactory = worldMapViewModelFactory;
        }

        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public ILocation Select(ISelectLocationParameter selectLocationParameter)
        {
            //WorldMapViewModel = _worldMapViewModelFactory.Create(selectLocationParameter.WorldMap, SelectLocation);

            //DispatcherFrame frame = new DispatcherFrame(true);
            

            return selectLocationParameter.AvailableLocations.First();
        }

        private void SelectLocation(ILocation location)
        {
            _selectedLocation = location;
        }
    }
}