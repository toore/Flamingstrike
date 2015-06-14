using System.Collections.Generic;

namespace RISK.Application.GameSetup
{
    public class GameSetupState
    {
        public GameSetupState(List<Player> players, List<GameboardTerritory> gameboardTerritories)
        {
            Players = players;
            GameboardTerritories = gameboardTerritories;
        }

        public List<Player> Players { get; }
        public List<GameboardTerritory> GameboardTerritories { get; }
    }
}