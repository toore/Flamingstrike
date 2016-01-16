using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface ITerritory : Application.ITerritory
    {
        int GetNumberOfArmiesAvailableForAttack();
        int GetNumberOfArmiesUsedForDefence();
    }

    public static class TerritoryExtensions
    {
        public static ITerritory Get(this IEnumerable<ITerritory> territories, ITerritoryId territoryId)
        {
            return territories.Single(x => x.TerritoryId == territoryId);
        }
    }

    public class Territory : Application.Territory, ITerritory
    {
        public Territory(ITerritoryId territoryId, IPlayerId playerId, int armies)
            : base(territoryId, playerId, armies) {}

        public int GetNumberOfArmiesAvailableForAttack()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesUsedForDefence()
        {
            return Armies;
        }
    }
}