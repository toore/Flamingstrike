using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;

namespace FlamingStrike.GameEngine.Setup
{
    public interface IAlternateGameSetupObserver
    {
        void SelectRegion(ITerritorySelector territorySelector);
        void NewGamePlaySetup(IGamePlaySetup gamePlaySetup);
    }
}