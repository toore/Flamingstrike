using System.Collections.Generic;

namespace RISK.Application.Setup
{
    public interface IGamePlaySetup
    {
        IReadOnlyList<IPlayerId> Players { get; }
        IReadOnlyList<ITerritory> Territories { get; }
    }

    public class GamePlaySetup : IGamePlaySetup
    {
        public GamePlaySetup(IReadOnlyList<IPlayerId> players, IReadOnlyList<ITerritory> territories)
        {
            Players = players;
            Territories = territories;
        }

        public IReadOnlyList<IPlayerId> Players { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
    }
}