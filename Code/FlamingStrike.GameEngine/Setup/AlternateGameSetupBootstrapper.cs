using System.Collections.Generic;
using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Setup
{
    public interface IAlternateGameSetupBootstrapper
    {
        void Run(IAlternateGameSetupObserver alternateGameSetupObserver, IReadOnlyList<PlayerName> players);
    }

    public class AlternateGameSetupBootstrapper : IAlternateGameSetupBootstrapper
    {
        private readonly IRegions _regions;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetupBootstrapper(IRegions regions, IShuffler shuffler, IStartingInfantryCalculator startingInfantryCalculator)
        {
            _regions = regions;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public void Run(IAlternateGameSetupObserver alternateGameSetupObserver, IReadOnlyList<PlayerName> players)
        {
            var alternateGameSetup = new AlternateGameSetup(
                alternateGameSetupObserver,
                _regions,
                _startingInfantryCalculator,
                _shuffler);

            alternateGameSetup.Run(players);
        }
    }
}