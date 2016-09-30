using System.Collections.Generic;
using Toore.Shuffling;

namespace RISK.GameEngine.Setup
{
    public interface IAlternateGameSetupFactory
    {
        IAlternateGameSetup Create(IGameSetupObserver gameSetupObserver, IEnumerable<IPlayer> players);
    }

    public class AlternateGameSetupFactory : IAlternateGameSetupFactory
    {
        private readonly IRegions _regions;
        private readonly IShuffle _shuffle;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetupFactory(IRegions regions, IShuffle shuffle, IStartingInfantryCalculator startingInfantryCalculator)
        {
            _regions = regions;
            _shuffle = shuffle;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IAlternateGameSetup Create(IGameSetupObserver gameSetupObserver, IEnumerable<IPlayer> players)
        {
            return new AlternateGameSetup(gameSetupObserver, _regions, players, _startingInfantryCalculator, _shuffle);
        }
    }
}