using System.Threading;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritoryId GetSelectedTerritory(ITerritoryRequestParameter territoryRequestParameter);
        void SelectTerritory(ITerritoryId location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritoryId _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritoryId GetSelectedTerritory(ITerritoryRequestParameter territoryRequestParameter)
        {
            // TODO: territoryRequestParameter is not used!
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectTerritory(ITerritoryId location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}