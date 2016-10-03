using System;

namespace RISK.GameEngine
{
    public interface ITerritory
    {
        IRegion Region { get; }
        IPlayer Player { get; }
        int Armies { get; }

        int GetNumberOfArmiesThatCanBeUsedInAnAttack();
        int GetNumberOfArmiesUsedAsDefence();
        int GetNumberOfArmiesThatCanFortifyAnotherTerritory();
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

        public int GetNumberOfArmiesThatCanBeUsedInAnAttack()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesUsedAsDefence()
        {
            return Armies;
        }

        public int GetNumberOfArmiesThatCanFortifyAnotherTerritory()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return Math.Max(Armies - 1, 0);
        }
    }
}