﻿using System.Collections.Generic;
using FlamingStrike.GameEngine.Shuffling;

namespace FlamingStrike.GameEngine.Setup
{
    public interface IAlternateGameSetupFactory
    {
        IAlternateGameSetup Create(IAlternateGameSetupObserver alternateGameSetupObserver, ICollection<IPlayer> players);
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

        public IAlternateGameSetup Create(IAlternateGameSetupObserver alternateGameSetupObserver, ICollection<IPlayer> players)
        {
            return new AlternateGameSetup(alternateGameSetupObserver, _regions, players, _startingInfantryCalculator, _shuffle);
        }
    }
}