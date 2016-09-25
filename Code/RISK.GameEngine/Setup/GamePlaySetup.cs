using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Setup
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<IPlayer> Players { get; }
        IReadOnlyList<ITerritory> Territories { get; }
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        public GamePlaySetup(IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories)
        {
            Players = players;
            Territories = territories;
        }

        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
    }
}