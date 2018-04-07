using System.Linq;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;

namespace Tests.GameEngine.Setup
{
    public class AutoRespondingAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(ITerritorySelector territorySelector)
        {
            var selectedRegion = territorySelector.GetTerritories().First(x => x.IsSelectable).Region;
            territorySelector.PlaceArmyInRegion(selectedRegion);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) { }
    }
}