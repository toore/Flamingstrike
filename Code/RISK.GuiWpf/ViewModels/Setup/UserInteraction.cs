using System.Threading;
using RISK.Core;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteraction
    {
        IRegion WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter);
        void SelectTerritory(IRegion region);
    }

    public class UserInteraction : IUserInteraction
    {
        private IRegion _selectedRegion;
        private readonly AutoResetEvent _territoryIdHasBeenSet = new AutoResetEvent(false);

        public IRegion WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter)
        {
            _territoryIdHasBeenSet.WaitOne();
            return _selectedRegion;
        }

        public void SelectTerritory(IRegion region)
        {
            _selectedRegion = region;
            _territoryIdHasBeenSet.Set();
        }
    }
}