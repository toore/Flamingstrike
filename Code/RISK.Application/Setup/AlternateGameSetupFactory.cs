using System.Collections.Generic;
using RISK.Application.Shuffling;
using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface IAlternateGameSetupFactory
    {
        IAlternateGameSetup Create(IEnumerable<IPlayer> players);
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

        public IAlternateGameSetup Create(IEnumerable<IPlayer> players)
        {
            return new AlternateGameSetup(_regions, players, _startingInfantryCalculator, _shuffler);
        }
    }
}