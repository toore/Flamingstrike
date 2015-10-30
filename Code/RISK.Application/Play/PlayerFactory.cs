using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play
{
    public interface IPlayerFactory
    {
        List<IPlayer> Create(IEnumerable<IPlayerId> players);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public List<IPlayer> Create(IEnumerable<IPlayerId> players)
        {
            return players
                .Select(Create)
                .ToList();
        }

        private static IPlayer Create(IPlayerId playerId)
        {
            return new Player(playerId);
        }
    }
}