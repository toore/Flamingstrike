﻿using System.Linq;
using RISK.GameEngine.Setup;

namespace Tests.RISK.GameEngine.Setup
{
    public class AutoRespondingAlternateGameSetupObserver : IAlternateGameSetupObserver
    {
        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            var selectedRegion = placeArmyRegionSelector.SelectableRegions.First();
            placeArmyRegionSelector.PlaceArmyInRegion(selectedRegion);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }
}