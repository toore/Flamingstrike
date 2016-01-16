using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play
{
    public interface IPlayerFactory
    {
        List<IInGameplayPlayer> Create(IEnumerable<IPlayer> players);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public List<IInGameplayPlayer> Create(IEnumerable<IPlayer> players)
        {
            return players
                .Select(Create)
                .ToList();
        }

        private static IInGameplayPlayer Create(IPlayer player)
        {
            return new InGameplayPlayer(player);
        }
    }
}