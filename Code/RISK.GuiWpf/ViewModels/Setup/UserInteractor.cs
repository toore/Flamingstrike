using System.Threading;
using RISK.Application;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritory GetSelectedTerritory(ITerritorySelectorParameter territorySelector);
        void SelectTerritory(ITerritory location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritory _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritory GetSelectedTerritory(ITerritorySelectorParameter territorySelector)
        {
            // territorySelector is not used!
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectTerritory(ITerritory location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}