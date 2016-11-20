using System.Linq;
using FlamingStrike.GameEngine.Setup;

namespace Tests.FlamingStrike.GameEngine.Setup
{
    public class AutoRespondingAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            var selectedRegion = placeArmyRegionSelector.SelectableRegions.First();
            placeArmyRegionSelector.PlaceArmyInRegion(selectedRegion);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }
}