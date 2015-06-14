using System.Threading;
using RISK.Application.GameSetup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritory GetSelectedTerritory(ITerritoryRequestParameter territoryRequest);
        void SelectTerritory(ITerritory location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritory _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritory GetSelectedTerritory(ITerritoryRequestParameter territoryRequest)
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