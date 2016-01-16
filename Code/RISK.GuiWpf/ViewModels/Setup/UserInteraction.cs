using System.Threading;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInteraction
    {
        ITerritoryGeography WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter);
        void SelectTerritory(ITerritoryGeography territoryGeography);
    }

    public class UserInteraction : IUserInteraction
    {
        private ITerritoryGeography _selectedTerritoryGeography;
        private readonly AutoResetEvent _territoryIdHasBeenSet = new AutoResetEvent(false);

        public ITerritoryGeography WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter)
        {
            _territoryIdHasBeenSet.WaitOne();
            return _selectedTerritoryGeography;
        }

        public void SelectTerritory(ITerritoryGeography territoryGeography)
        {
            _selectedTerritoryGeography = territoryGeography;
            _territoryIdHasBeenSet.Set();
        }
    }
}