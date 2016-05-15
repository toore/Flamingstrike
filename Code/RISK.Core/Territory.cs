using System;

namespace RISK.Core
{
    public interface ITerritory
    {
        IRegion Region { get; }
        IPlayer Player { get; }
        int Armies { get; }

        int GetNumberOfArmiesAvailableForAttack();
        int GetNumberOfArmiesUsedForDefence();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
    }

    public class Territory : ITerritory
    {
        public Territory(IRegion region, IPlayer player, int armies)
        {
            if (armies <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armies), armies, "A territory can't have equal or less than zero armies.");
            }

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

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return Math.Max(Armies - 1, 0);
        }
    }
}