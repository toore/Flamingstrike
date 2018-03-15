using FlamingStrike.GameEngine.Play.GameStates;
using FlamingStrike.GameEngine.Setup;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGameFactory
    {
        IGame Create(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;
        private readonly IArmyDrafter _armyDrafter;

        public GameFactory(
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory,
            IArmyDrafter armyDrafter)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
            _armyDrafter = armyDrafter;
        }

        public IGame Create(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            return new Game(
                gameObserver,
                _gameStateFactory,
                _armyDraftCalculator,
                _deckFactory,
                gamePlaySetup.Territories,
                gamePlaySetup.Players,
                _armyDrafter);
        }
    }
}