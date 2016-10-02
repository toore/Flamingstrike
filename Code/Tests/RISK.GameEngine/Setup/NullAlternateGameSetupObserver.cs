using RISK.GameEngine.Setup;

namespace Tests.RISK.GameEngine.Setup
{
    internal class NullAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector) {}

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }
}