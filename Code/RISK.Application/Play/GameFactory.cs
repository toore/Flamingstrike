using RISK.Application.Play.GamePhases;
using RISK.Application.Setup;

namespace RISK.Application.Play
{
    public interface IGameFactory
    {
        IGame Create(IGamePlaySetup gamePlaySetup);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IDeckFactory _deckFactory;

        public GameFactory(IGameStateConductor gameStateConductor, IDeckFactory deckFactory)
        {
            _gameStateConductor = gameStateConductor;
            _deckFactory = deckFactory;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(
                _gameStateConductor,
                gamePlaySetup.Players,
                gamePlaySetup.Territories,
                _deckFactory.Create());

            return game;
        }
    }
}