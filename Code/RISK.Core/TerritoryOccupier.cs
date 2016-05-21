using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface ITerritoryOccupier
    {
        IReadOnlyList<ITerritory> SendInAdditionalArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion occupiedRegion, int numberOfAdditionalArmiesToSendIn);
    }

    public class TerritoryOccupier : ITerritoryOccupier
    {
        public IReadOnlyList<ITerritory> SendInAdditionalArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion occupiedRegion, int numberOfAdditionalArmiesToSendIn)
        {
            var attackingTerritory = territories.Single(x => x.Region == attackingRegion);
            var updatedAttackingArmies = attackingTerritory.Armies - numberOfAdditionalArmiesToSendIn;
            var updatedAttackingTerritory = new Territory(attackingRegion, attackingTerritory.Player, updatedAttackingArmies);

            var occupiedTerritory = territories.Single(x => x.Region == occupiedRegion);
            var updatedOccupiedArmies = occupiedTerritory.Armies + numberOfAdditionalArmiesToSendIn;
            var updatedOccupiedTerritory = new Territory(occupiedRegion, occupiedTerritory.Player, updatedOccupiedArmies);

            var updatedTerritories = territories
                .Replace(attackingTerritory, updatedAttackingTerritory)
                .Replace(occupiedTerritory, updatedOccupiedTerritory)
                .ToList();

            return updatedTerritories;
        }
    }
}