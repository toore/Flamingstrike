using RISK.GameEngine.Play.GamePhases;
using RISK.GameEngine.Setup;

namespace RISK.GameEngine.Play
{
    public interface IGameFactory
    {
        IGame Create(IGamePlaySetup gamePlaySetup);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IDeckFactory _deckFactory;
        private readonly IGameStateFsm _gameStateFsm;

        public GameFactory(
            IGameDataFactory gameDataFactory,
            IGameStateConductor gameStateConductor,
            IDeckFactory deckFactory,
            IGameStateFsm gameStateFsm)
        {
            _gameDataFactory = gameDataFactory;
            _gameStateConductor = gameStateConductor;
            _deckFactory = deckFactory;
            _gameStateFsm = gameStateFsm;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(
                _gameDataFactory,
                _gameStateConductor,
                gamePlaySetup.Players,
                gamePlaySetup.Territories,
                _deckFactory.Create(),
                _gameStateFsm);

            game.Initialize();

            return game;
        }
    }
}