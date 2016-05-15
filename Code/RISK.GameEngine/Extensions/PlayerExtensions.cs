using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Play;

namespace RISK.GameEngine.Extensions
{
    public static class PlayerExtensions
    {
        public static IInGamePlayer GetPlayer(this IEnumerable<IInGamePlayer> players, IPlayer player)
        {
            return players.Single(x => x.Player == player);
        }
    }
}