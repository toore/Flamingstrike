using System.Threading;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ITerritory GetLocation(ILocationSelectorParameter locationSelector);
        void SelectLocation(ITerritory location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ITerritory _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ITerritory GetLocation(ILocationSelectorParameter locationSelector)
        {
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectLocation(ITerritory location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}