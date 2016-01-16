using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play
{
    public interface ITerritoryFactory
    {
        List<Territory> Create(IEnumerable<Application.ITerritory> territories);
    }

    public class TerritoryFactory : ITerritoryFactory
    {
        public List<Territory> Create(IEnumerable<Application.ITerritory> territories)
        {
            return territories
                .Select(Create)
                .ToList();
        }

        private static Territory Create(Application.ITerritory territory)
        {
            return new Territory(
                territory.TerritoryId,
                territory.Player,
                territory.Armies);
        }
    }
}