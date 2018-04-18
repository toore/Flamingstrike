using System;

namespace FlamingStrike.GameEngine.Play
{
    public interface ITerritory
    {
        Region Region { get; }
        PlayerName Name { get; }
        int Armies { get; }
        int GetNumberOfArmiesThatCanAttack();
        int GetNumberOfArmiesUsedInAnAttack();
        int GetNumberOfDefendingArmies();
        int GetNumberOfArmiesThatCanFortifyAnotherTerritory();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
        void Occupy(PlayerName name, int armies);
        void RemoveArmies(int armiesLost);
    }

    public class Territory : ITerritory
    {
        private const int MaxNumberOfAttackingArmies = 3;
        private const int MaxNumberOfDefendingArmies = 2;

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
        public PlayerName Name { get; private set; }
        public int Armies { get; private set; }

        public int GetNumberOfArmiesThatCanAttack()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesUsedInAnAttack()
        {
            return Math.Min(MaxNumberOfAttackingArmies, GetNumberOfArmiesThatCanAttack());
        }

        public int GetNumberOfDefendingArmies()
        {
            return Math.Min(Armies, MaxNumberOfDefendingArmies);
        }

        public int GetNumberOfArmiesThatCanFortifyAnotherTerritory()
        {
            return Math.Max(Armies - 1, 0);
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return Math.Max(Armies - 1, 0);
        }

        public void Occupy(PlayerName name, int armies)
        {
            Name = name;
            Armies = armies;
        }

        public void RemoveArmies(int numberOfArmiesLost)
        {
            if (numberOfArmiesLost > Armies)
            {
                throw new InvalidOperationException("Can't remove more armies than exist");
            }

            if (numberOfArmiesLost == Armies)
            {
                throw new InvalidOperationException("Can't remove all armies without being occupied");
            }

            Armies = Armies - numberOfArmiesLost;
        }
    }
}