using System.Collections.Generic;
using RISK.Core;
using Toore.Shuffling;

namespace RISK.Application.Setup
{
    public interface IAlternateGameSetupFactory
    {
        IAlternateGameSetup Create(IEnumerable<IPlayer> players);
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

        public IAlternateGameSetup Create(IEnumerable<IPlayer> players)
        {
            return new AlternateGameSetup(_regions, players, _startingInfantryCalculator, _shuffle);
        }
    }
}