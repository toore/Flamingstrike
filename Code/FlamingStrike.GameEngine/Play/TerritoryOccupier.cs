using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface ITerritoryOccupier
    {
        IReadOnlyList<ITerritory> SendInAdditionalArmiesToOccupy(IReadOnlyList<ITerritory> territories, Region attackingRegion, Region occupiedRegion, int numberOfAdditionalArmiesToSendIn);
    }

    public class TerritoryOccupier : ITerritoryOccupier
    {
        public IReadOnlyList<ITerritory> SendInAdditionalArmiesToOccupy(IReadOnlyList<ITerritory> territories, Region attackingRegion, Region occupiedRegion, int numberOfAdditionalArmiesToSendIn)
        {
            var attackingTerritory = territories.Single(x => x.Region == attackingRegion);
            var occupiedTerritory = territories.Single(x => x.Region == occupiedRegion);

            var armiesLeft = attackingTerritory.Armies - numberOfAdditionalArmiesToSendIn;
            var armiesOccupying = occupiedTerritory.Armies + numberOfAdditionalArmiesToSendIn;

            var updatedAttackingTerritory = new Territory(attackingRegion, attackingTerritory.Name, armiesLeft);
            var updatedOccupiedTerritory = new Territory(occupiedRegion, occupiedTerritory.Name, armiesOccupying);

            return territories
                .Except(new[] { attackingTerritory, occupiedTerritory })
                .Union(new[] { updatedAttackingTerritory, updatedOccupiedTerritory })
                .ToList();
        }
    }
}