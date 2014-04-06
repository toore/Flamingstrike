using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.Setup
{
    public static class SetupArmiesExtensions
    {
        public static bool AnyArmiesLeft(this IList<SetupPlayer> players)
        {
            return players.Any(x => x.GetArmies() > 0);
        }
    }
}