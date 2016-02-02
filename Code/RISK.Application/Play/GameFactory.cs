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
        private readonly IGameStateFactory _gameStateFactory;

        public GameFactory(IGameStateFactory gameStateFactory)
        {
            _gameStateFactory = gameStateFactory;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(_gameStateFactory, gamePlaySetup.Players, gamePlaySetup.Territories);

            return game;
        }
    }
}