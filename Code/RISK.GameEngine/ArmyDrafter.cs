using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine
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
            var reinforcedTerritory = new Territory(regionToReinforce, territoryToReinforce.Player, currentArmies + numberOfArmiesToPlace);

            return territories
                .Except(new[] { territoryToReinforce })
                .Union(new[] { reinforcedTerritory })
                .ToList();
        }
    }
}