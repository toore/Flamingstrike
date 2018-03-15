using FlamingStrike.GameEngine.Setup;

namespace FlamingStrike.GameEngine.Play
{
    public interface IGameBootstrapper
    {
        void Run(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup);
    }

    public class GameBootstrapper : IGameBootstrapper
    {
        private readonly IGamePhaseFactory _gamePhaseFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly IDeckFactory _deckFactory;

        public GameBootstrapper(
            IGamePhaseFactory gamePhaseFactory,
            IArmyDraftCalculator armyDraftCalculator,
            IDeckFactory deckFactory)
        {
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
            _gamePhaseFactory = gamePhaseFactory;
        }

        public void Run(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(
                gameObserver,
                _gamePhaseFactory,
                _armyDraftCalculator,
                _deckFactory,
                gamePlaySetup.Territories,
                gamePlaySetup.Players);

            game.Start();
        }
    }
}