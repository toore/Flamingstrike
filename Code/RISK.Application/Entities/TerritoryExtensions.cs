using System;

namespace RISK.Domain.Entities
{
    public static class TerritoryExtensions
    {
        public static bool IsOccupied(this ITerritory territory)
        {
            return territory.Occupant != null;
        }

        public static bool HasArmiesToAttackWith(this ITerritory territory)
        {
            return territory.GetArmiesToAttackWith() > 0;
        }

        public static int GetArmiesToAttackWith(this ITerritory territory)
        {
            return Math.Max(territory.Armies - 1, 0);
        }
    }
}