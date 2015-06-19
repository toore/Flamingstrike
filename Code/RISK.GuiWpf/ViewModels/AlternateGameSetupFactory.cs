﻿using System.Collections.Generic;
using RISK.Application;
using RISK.Application.GameSetup;
using RISK.Application.Shuffling;
using RISK.Application.World;

namespace GuiWpf.ViewModels
{
    public class AlternateGameSetupFactory
    {
        private readonly IWorldMap _worldMap;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetupFactory(IWorldMap worldMap, IShuffler shuffler, IStartingInfantryCalculator startingInfantryCalculator)
        {
            _worldMap = worldMap;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IAlternateGameSetup Create(IEnumerable<IPlayer> players)
        {
            return new AlternateGameSetup(_worldMap, players, _startingInfantryCalculator, _shuffler);
        }
    }
}