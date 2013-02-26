using RISK.Domain.Entities;

namespace RISK.Domain.Extensions
{
    public static class TerritoryExtensions
    {
        public static bool HasOwner(this ITerritory territory)
        {
            return territory.Owner != null;
        }
    }
}