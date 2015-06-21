using System.Collections.Generic;

namespace RISK.Application.Setup
{
    public interface IGameSetup
    {
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<IGameboardSetupTerritory> GameboardTerritories { get; }
    }

    public class GameSetup : IGameSetup
    {
        public GameSetup(IReadOnlyList<IPlayer> players, IReadOnlyList<IGameboardSetupTerritory> gameboardTerritories)
        {
            Players = players;
            GameboardTerritories = gameboardTerritories;
        }

        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<IGameboardSetupTerritory> GameboardTerritories { get; }
    }
}