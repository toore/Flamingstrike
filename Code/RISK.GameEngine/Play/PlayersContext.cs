using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface IPlayersContext
    {
        void Set(IReadOnlyList<IPlayer> players);
        IReadOnlyList<IPlayer> Players { get; }
    }

    public class PlayersContext : IPlayersContext
    {
        public void Set(IReadOnlyList<IPlayer> players)
        {
            Players = players;
        }

        public IReadOnlyList<IPlayer> Players { get; private set; }
    }
}