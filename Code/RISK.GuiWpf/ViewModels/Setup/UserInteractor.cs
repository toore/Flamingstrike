using System.Threading;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteractor
    {
        ILocation GetLocation(ILocationSelectorParameter locationSelector);
        void SelectLocation(ILocation location);
    }

    public class UserInteractor : IUserInteractor
    {
        private ILocation _selectedLocation;
        private readonly AutoResetEvent _locationHasBeenSelected = new AutoResetEvent(false);

        public ILocation GetLocation(ILocationSelectorParameter locationSelector)
        {
            _locationHasBeenSelected.WaitOne();
            return _selectedLocation;
        }

        public void SelectLocation(ILocation location)
        {
            _selectedLocation = location;
            _locationHasBeenSelected.Set();
        }
    }
}