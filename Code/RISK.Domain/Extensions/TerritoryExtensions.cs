using RISK.Domain.Entities;

namespace RISK.Domain.Extensions
{
    public static class TerritoryExtensions
    {
        public static bool IsAssignedToPlayer(this ITerritory territory)
        {
            return territory.AssignedToPlayer != null;
        }
    }
}