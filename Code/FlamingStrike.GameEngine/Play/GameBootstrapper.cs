using System.Linq;
using FlamingStrike.GameEngine.Setup.Finished;

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
                _deckFactory);

            var territories = gamePlaySetup.GetTerritories()
                .Select(x => new Territory(x.Region, x.Name, x.Armies))
                .ToList();

            game.Run(territories, gamePlaySetup.GetPlayers());
        }
    }
}