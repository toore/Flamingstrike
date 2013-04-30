using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.Setup
{
    public static class PlayerDuringSetupExtensions
    {
        public static bool AnyArmiesLeft(this IList<PlayerDuringSetup> players)
        {
            return players.Any(x => x.Armies > 0);
        }
    }
}