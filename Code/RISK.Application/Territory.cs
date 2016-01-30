using System;
using RISK.Application.World;

namespace RISK.Application
{
    public interface ITerritory
    {
        IRegion Region { get; }
        IPlayer Player { get; }
        int Armies { get; }

        int GetNumberOfArmiesAvailableForAttack();
        int GetNumberOfArmiesUsedForDefence();
    }

    public class Territory : ITerritory
    {
        public Territory(IRegion region, IPlayer player, int armies)
        {
            Region = region;
            Player = player;
            Armies = armies;
        }

        public IRegion Region { get; }
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