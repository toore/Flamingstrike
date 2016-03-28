using RISK.Application.Play.GamePhases;
using RISK.Application.Play.Planning;
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
        private readonly IArmyDraftCalculator _armyDraftCalculator;

        public GameFactory(IGameStateFactory gameStateFactory, IArmyDraftCalculator armyDraftCalculator)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(_gameStateFactory, _armyDraftCalculator, gamePlaySetup.Players, gamePlaySetup.Territories);

            return game;
        }
    }
}