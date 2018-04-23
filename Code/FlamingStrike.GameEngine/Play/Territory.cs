using System;

namespace FlamingStrike.GameEngine.Play
{
    public interface ITerritory
    {
        Region Region { get; }
        PlayerName PlayerName { get; }
        int Armies { get; }
        int GetNumberOfArmiesThatCanAttack();
        int GetNumberOfArmiesUsedInAnAttack();
        int GetNumberOfDefendingArmies();
        int GetNumberOfArmiesThatCanFortifyAnotherTerritory();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
        void Occupy(PlayerName name, int armies);
        void RemoveArmies(int numberOfArmies);
        void AddArmies(int numberOfArmies);
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
            PlayerName = playerName;
            Armies = armies;
        }

        public Region Region { get; }
        public PlayerName PlayerName { get; private set; }
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
            PlayerName = name;
            Armies = armies;
        }

        public void RemoveArmies(int numberOfArmies)
        {
            if (numberOfArmies <= 0)
            {
                throw new InvalidOperationException("Can't remove zero or less armies.");
            }

            if (numberOfArmies > Armies)
            {
                throw new InvalidOperationException("Can't remove more armies than exist");
            }

            if (numberOfArmies == Armies)
            {
                throw new InvalidOperationException("Can't remove all armies without being occupied");
            }

            Armies = Armies - numberOfArmies;
        }

        public void AddArmies(int numberOfArmies)
        {
            if (numberOfArmies <= 0)
            {
                throw new InvalidOperationException("Can't add zero or less armies.");
            }

            Armies += numberOfArmies;
        }
    }
}