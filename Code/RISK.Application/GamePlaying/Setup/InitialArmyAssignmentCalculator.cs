﻿namespace RISK.Application.GamePlaying.Setup
{
    public class InitialArmyAssignmentCalculator : IInitialArmyAssignmentCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}