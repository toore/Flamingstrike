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
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;

        public GameFactory(
            IGamePhaseFactory gamePhaseFactory,
            IGameStateFactory gameStateFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
            _gamePhaseFactory = gamePhaseFactory;
        }

        public IGame Create(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            return new Game(
                gameObserver,
                _gameStateFactory,
                _gamePhaseFactory, _armyDraftCalculator, _deckFactory, gamePlaySetup.Territories, gamePlaySetup.Players);
        }
    }
}