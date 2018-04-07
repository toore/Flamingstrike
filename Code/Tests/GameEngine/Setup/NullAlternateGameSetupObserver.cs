using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;

namespace Tests.GameEngine.Setup
{
    internal class NullAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(ITerritorySelector territorySelector) {}

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }
}