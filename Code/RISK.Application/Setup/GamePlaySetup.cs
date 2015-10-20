using System.Collections.Generic;

namespace RISK.Application.Setup
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<IGameboardSetupTerritory> GameboardSetupTerritories { get; }
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        public GamePlaySetup(IReadOnlyList<IPlayer> players, IReadOnlyList<IGameboardSetupTerritory> gameboardSetupTerritories)
        {
            Players = players;
            GameboardSetupTerritories = gameboardSetupTerritories;
        }

        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<IGameboardSetupTerritory> GameboardSetupTerritories { get; }
    }
}