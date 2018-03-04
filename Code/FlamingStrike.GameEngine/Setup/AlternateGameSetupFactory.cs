using System.Collections.Generic;
using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Setup
{
    public interface IAlternateGameSetupFactory
    {
        IAlternateGameSetup Create(IAlternateGameSetupObserver alternateGameSetupObserver, ICollection<IPlayer> players);
    }

    public class AlternateGameSetupFactory : IAlternateGameSetupFactory
    {
        private readonly IRegions _regions;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetupFactory(IRegions regions, IShuffler shuffler, IStartingInfantryCalculator startingInfantryCalculator)
        {
            _regions = regions;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IAlternateGameSetup Create(IAlternateGameSetupObserver alternateGameSetupObserver, ICollection<IPlayer> players)
        {
            return new AlternateGameSetup(alternateGameSetupObserver, _regions, players, _startingInfantryCalculator, _shuffler);
        }
    }
}