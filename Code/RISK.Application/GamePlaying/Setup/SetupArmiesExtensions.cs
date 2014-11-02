﻿using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.GamePlaying.Setup
{
    public static class SetupArmiesExtensions
    {
        public static bool AnyArmiesLeft(this IList<PlayerDuringGameSetup> players)
        {
            return players.Any(x => x.GetArmiesLeft() > 0);
        }
    }
}