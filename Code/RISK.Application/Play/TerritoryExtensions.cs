using System.Collections.Generic;
using System.Linq;
using RISK.Core;

namespace RISK.Application.Play
{
    public static class TerritoryExtensions
    {
        public static ITerritory GetTerritory(this IEnumerable<ITerritory> territories, IRegion region)
        {
            return territories.Single(x => x.Region == region);
        }
    }
}