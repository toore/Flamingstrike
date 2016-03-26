using System.Collections.Generic;

namespace RISK.Application.Setup
{
    public interface IGamePlaySetup
    {
        Sequence<IPlayer> Players { get; }
        IReadOnlyList<ITerritory> Territories { get; }
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        public GamePlaySetup(Sequence<IPlayer> players, IReadOnlyList<ITerritory> territories)
        {
            Players = players;
            Territories = territories;
        }

        public Sequence<IPlayer> Players { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
    }
}