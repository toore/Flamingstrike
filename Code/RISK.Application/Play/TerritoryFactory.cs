using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play
{
    public interface ITerritoryFactory
    {
        List<ITerritory> Create(IEnumerable<Application.ITerritory> territories);
    }

    public class TerritoryFactory : ITerritoryFactory
    {
        public List<ITerritory> Create(IEnumerable<Application.ITerritory> territories)
        {
            return territories
                .Select(Create)
                .ToList();
        }

        private static ITerritory Create(Application.ITerritory territory)
        {
            return new Territory(
                territory.TerritoryGeography,
                territory.Player,
                territory.Armies);
        }
    }
}