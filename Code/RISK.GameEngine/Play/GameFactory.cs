using System.Linq;
using RISK.Core;
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
        private readonly IGameContext _gameContext;
        private readonly IGameRules _gameRules;

        public GameFactory(
            IGameDataFactory gameDataFactory,
            IGameStateConductor gameStateConductor,
            IDeckFactory deckFactory,
            IGameContext gameContext,
            IGameRules gameRules)
        {
            _gameDataFactory = gameDataFactory;
            _gameStateConductor = gameStateConductor;
            _deckFactory = deckFactory;
            _gameContext = gameContext;
            _gameRules = gameRules;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var players = gamePlaySetup.Players;

            var gameData = _gameDataFactory.Create(
                players.Next(),
                players.ToList(),
                gamePlaySetup.Territories,
                _deckFactory.Create());

            _gameStateConductor.CurrentPlayerStartsNewTurn(gameData);

            return new Game(_gameContext, _gameRules);
        }
    }
}