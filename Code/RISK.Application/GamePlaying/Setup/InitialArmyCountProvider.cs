﻿namespace RISK.Domain.GamePlaying.Setup
{
    public class InitialArmyCountProvider : IInitialArmyCountProvider
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}