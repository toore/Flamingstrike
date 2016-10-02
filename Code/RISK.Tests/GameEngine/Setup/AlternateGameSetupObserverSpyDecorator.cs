using RISK.GameEngine.Setup;

namespace RISK.Tests.GameEngine.Setup
{
    internal interface IAlternateGameSetupObserverSpy : IAlternateGameSetupObserver
    {
        IPlaceArmyRegionSelector PlaceArmyRegionSelector { get; }
        IGamePlaySetup GamePlaySetup { get; }
    }

    public class AlternateGameSetupObserverSpyDecorator : IAlternateGameSetupObserverSpy
    {
        private readonly IAlternateGameSetupObserver _alternateGameSetupObserverToBeDecorated;

        public AlternateGameSetupObserverSpyDecorator(IAlternateGameSetupObserver alternateGameSetupObserverToBeDecorated)
        {
            _alternateGameSetupObserverToBeDecorated = alternateGameSetupObserverToBeDecorated;
        }

        public IPlaceArmyRegionSelector PlaceArmyRegionSelector { get; private set; }
        public IGamePlaySetup GamePlaySetup { get; private set; }

        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            PlaceArmyRegionSelector = placeArmyRegionSelector;
            _alternateGameSetupObserverToBeDecorated.SelectRegion(placeArmyRegionSelector);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            GamePlaySetup = gamePlaySetup;
            _alternateGameSetupObserverToBeDecorated.NewGamePlaySetup(gamePlaySetup);
        }
    }
}