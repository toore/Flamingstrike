using System;

namespace FlamingStrike.GameEngine.Play
{
    public interface ITerritory
    {
        Region Region { get; }
        PlayerName Name { get; }
        int Armies { get; }
        int GetNumberOfArmiesThatAreAvailableForAnAttack();
        int GetNumberOfArmiesUsedInAnAttack();
        int GetNumberOfArmiesUsedAsDefence();
        int GetNumberOfArmiesThatCanFortifyAnotherTerritory();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
    }

    public class Territory : ITerritory
    {
        private const int MaxNumberOfAttackingArmies = 3;

        public Territory(Region region, PlayerName playerName, int armies)
        {
            if (armies <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armies), armies, "A territory can't have equal or less than zero armies.");
            }

            Region = region;
            Name = playerName;
            Armies = armies;
        }

        public Region Region { get; }
        public PlayerName Name { get; }
        public int Armies { get; }

        public int GetNumberOfArmiesThatAreAvailableForAnAttack()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesUsedInAnAttack()
        {
            return Math.Min(MaxNumberOfAttackingArmies, GetNumberOfArmiesThatAreAvailableForAnAttack());
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