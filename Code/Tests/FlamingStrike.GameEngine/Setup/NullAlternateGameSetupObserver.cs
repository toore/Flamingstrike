using FlamingStrike.GameEngine.Setup;

namespace Tests.FlamingStrike.GameEngine.Setup
{
    internal class NullAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector) {}

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }
}