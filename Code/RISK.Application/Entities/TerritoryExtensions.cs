using System;

namespace RISK.Domain.Entities
{
    public static class TerritoryExtensions
    {
        public static bool IsOccupied(this ITerritory territory)
        {
            return territory.Occupant != null;
        }

        public static bool HasArmiesAvailableForAttack(this ITerritory territory)
        {
            return territory.GetArmiesAvailableForAttack() > 0;
        }

        public static int GetArmiesAvailableForAttack(this ITerritory territory)
        {
            return Math.Max(territory.Armies - 1, 0);
        }
    }
}