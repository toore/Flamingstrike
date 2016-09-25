using RISK.Core;
using RISK.GameEngine.Play.GameStates;
using RISK.GameEngine.Setup;

namespace RISK.GameEngine.Play
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

        public GameFactory(
            IGameStateFactory gameStateFactory, 
            IArmyDraftCalculator armyDraftCalculator, 
            IDeckFactory deckFactory)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;
            _deckFactory = deckFactory;
        }

        public IGame Create(IGameObserver gameObserver, IGamePlaySetup gamePlaySetup)
        {
            return new Game(
                gameObserver,
                _gameStateFactory,
                _armyDraftCalculator,
                _deckFactory.Create(),
                gamePlaySetup.Players,
                gamePlaySetup.Territories);
        }
    }
}