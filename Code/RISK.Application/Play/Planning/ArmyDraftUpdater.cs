using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.World;

namespace RISK.Application.Play.Planning
{
    public interface IArmyDraftUpdater
    {
        IReadOnlyList<ITerritory> PlaceArmies(IReadOnlyList<ITerritory> territories, IRegion region, int numberOfArmiesToPlace);
    }

    public class ArmyDraftUpdater : IArmyDraftUpdater
    {
        public IReadOnlyList<ITerritory> PlaceArmies(IReadOnlyList<ITerritory> territories, IRegion region, int numberOfArmiesToPlace)
        {
            var territoryToUpdate = territories.Single(x => x.Region == region);
            var currentNumberOfArmies = territoryToUpdate.Armies;
            var updatedTerritory = new Territory(region, territoryToUpdate.Player, currentNumberOfArmies + numberOfArmiesToPlace);

            var updatedTerritories = territories.Update(territoryToUpdate, updatedTerritory)
                .ToList();

            return updatedTerritories;
        }
    }
}