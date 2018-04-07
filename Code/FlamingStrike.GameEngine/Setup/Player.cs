using System;

namespace FlamingStrike.GameEngine.Setup
{
    public class Player
    {
        public Player(string name, int armiesToPlace)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{nameof(name)} is null or white space.", nameof(name));
            }

            if (armiesToPlace < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armiesToPlace), armiesToPlace, "Value must be greater than or equal to zero.");
            }

            Name = name;
            ArmiesToPlace = armiesToPlace;
        }

        public string Name { get; }

        public int ArmiesToPlace { get; private set; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }

        public void ArmyPlaced()
        {
            ArmiesToPlace--;
        }
    }
}