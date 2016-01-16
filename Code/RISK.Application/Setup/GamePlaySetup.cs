using System.Collections.Generic;

namespace RISK.Application.Setup
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