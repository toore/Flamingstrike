using System.Collections.Generic;
using System.Linq;
using RISK.Core;

namespace RISK.GameEngine.Extensions
{
    public static class TerritoryExtensions
    {
        public static ITerritory GetTerritory(this IEnumerable<ITerritory> territories, IRegion region)
        {
            return territories.Single(x => x.Region == region);
        }
    }
}