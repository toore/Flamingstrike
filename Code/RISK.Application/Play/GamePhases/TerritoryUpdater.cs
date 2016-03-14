using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface ITerritoryUpdater
    {
        IReadOnlyList<ITerritory> PlaceArmies(IReadOnlyList<ITerritory> territories, IRegion region, int numberOfArmiesToPlace);
    }

    public class TerritoryUpdater : ITerritoryUpdater
    {
        public IReadOnlyList<ITerritory> PlaceArmies(IReadOnlyList<ITerritory> territories, IRegion region, int numberOfArmiesToPlace)
        {
            var territoryToUpdate = territories.Single(x => x.Region == region);
            var currentNumberOfArmies = territoryToUpdate.Armies;
            var updatedTerritory = new Territory(region, territoryToUpdate.Player, currentNumberOfArmies + numberOfArmiesToPlace);

            var updatedTerritories = territories
                .Except(new[] { territoryToUpdate })
                .Union(new[] { updatedTerritory })
                .ToList();

            return updatedTerritories;
        }
    }
}