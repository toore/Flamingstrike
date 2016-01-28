using System;
using RISK.Application.World;

namespace RISK.Application
{
    public interface ITerritory
    {
        ITerritoryGeography TerritoryGeography { get; }
        IPlayer Player { get; }
        int Armies { get; }

        int GetNumberOfArmiesAvailableForAttack();
        int GetNumberOfArmiesUsedForDefence();
    }

    public class Territory : ITerritory
    {
        public Territory(ITerritoryGeography territoryGeography, IPlayer player, int armies)
        {
            TerritoryGeography = territoryGeography;
            Player = player;
            Armies = armies;
        }

        public ITerritoryGeography TerritoryGeography { get; }
        public IPlayer Player { get; }
        public int Armies { get; set; }

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