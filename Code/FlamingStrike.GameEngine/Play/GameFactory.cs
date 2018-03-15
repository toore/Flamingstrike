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
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;

        public GameFactory(
            IGamePhaseFactory gamePhaseFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory)
        {
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
            _gamePhaseFactory = gamePhaseFactory;
        }

        public IGame Create(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            return new Game(
                gameObserver,
                _gamePhaseFactory,
                _armyDraftCalculator,
                _deckFactory,
                gamePlaySetup.Territories,
                gamePlaySetup.Players);
        }
    }
}