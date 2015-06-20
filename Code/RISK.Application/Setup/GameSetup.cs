using System.Collections.Generic;

namespace RISK.Application.Setup
{
    public interface IGameSetup
    {
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
    }

    public class GameSetup : IGameSetup
    {
        public GameSetup(IReadOnlyList<IPlayer> players, IReadOnlyList<GameboardTerritory> gameboardTerritories)
        {
            Players = players;
            GameboardTerritories = gameboardTerritories;
        }

        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
    }
}