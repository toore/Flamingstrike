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
        private readonly IDeckFactory _deckFactory;

        public GameFactory(IGameStateFactory gameStateFactory, IArmyDraftCalculator armyDraftCalculator, IDeckFactory deckFactory)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(
                _gameStateFactory,
                _armyDraftCalculator,
                gamePlaySetup.Players,
                gamePlaySetup.Territories,
                _deckFactory.Create());

            return game;
        }
    }
}