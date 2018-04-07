using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;

namespace Tests.GameEngine.Setup
{
    internal interface IAlternateGameSetupObserverSpy : IAlternateGameSetupObserver
    {
        ITerritorySelector TerritorySelector { get; }
        IGamePlaySetup GamePlaySetup { get; }
    }

    public class AlternateGameSetupObserverSpyDecorator : IAlternateGameSetupObserverSpy
    {
        private readonly IAlternateGameSetupObserver _alternateGameSetupObserverToBeDecorated;

        public AlternateGameSetupObserverSpyDecorator(IAlternateGameSetupObserver alternateGameSetupObserverToBeDecorated)
        {
            _alternateGameSetupObserverToBeDecorated = alternateGameSetupObserverToBeDecorated;
        }

        public ITerritorySelector TerritorySelector { get; private set; }
        public IGamePlaySetup GamePlaySetup { get; private set; }

        public void SelectRegion(ITerritorySelector territorySelector)
        {
            TerritorySelector = territorySelector;
            _alternateGameSetupObserverToBeDecorated.SelectRegion(territorySelector);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            GamePlaySetup = gamePlaySetup;
            _alternateGameSetupObserverToBeDecorated.NewGamePlaySetup(gamePlaySetup);
        }
    }
}