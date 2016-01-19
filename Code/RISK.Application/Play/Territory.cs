using System;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface ITerritory : Application.ITerritory
    {
        int GetNumberOfArmiesAvailableForAttack();
        int GetNumberOfArmiesUsedForDefence();
    }

    public class Territory : Application.Territory, ITerritory
    {
        public Territory(ITerritoryGeography territoryGeography, IPlayer player, int armies)
            : base(territoryGeography, player, armies) {}

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