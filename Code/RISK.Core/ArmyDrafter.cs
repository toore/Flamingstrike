using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IArmyDrafter
    {
        IReadOnlyList<ITerritory> PlaceDraftArmies(IReadOnlyList<ITerritory> territories, IRegion regionToReinforce, int numberOfArmiesToPlace);
    }

    public class ArmyDrafter : IArmyDrafter
    {
        public IReadOnlyList<ITerritory> PlaceDraftArmies(IReadOnlyList<ITerritory> territories, IRegion regionToReinforce, int numberOfArmiesToPlace)
        {
            var territoryToReinforce = territories.Single(x => x.Region == regionToReinforce);
            var currentArmies = territoryToReinforce.Armies;
            var updatedTerritory = new Territory(regionToReinforce, territoryToReinforce.Player, currentArmies + numberOfArmiesToPlace);

            var updatedTerritories = territories
                .Replace(territoryToReinforce, updatedTerritory)
                .ToList();

            return updatedTerritories;
        }
    }
}