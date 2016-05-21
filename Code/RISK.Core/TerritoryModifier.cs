using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface ITerritoryModifier
    {
        IReadOnlyList<ITerritory> PlaceDraftArmies(IReadOnlyList<ITerritory> territories, IRegion regionToReinforce, int numberOfArmiesToPlace);
        IReadOnlyList<ITerritory> SendInAdditionalArmiesToOccupy(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion occupiedRegion, int numberOfAdditionalArmiesToSendIn);
    }

    public class TerritoryModifier : ITerritoryModifier
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

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, T exclude, T include)
        {
            var updatedList = items
                .Except(new[] { exclude })
                .Union(new[] { include });

            return updatedList;
        }
    }
}