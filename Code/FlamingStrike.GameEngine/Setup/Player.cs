using System;

namespace FlamingStrike.GameEngine.Setup
{
    public class Player
    {
        public Player(PlayerName name, int armiesToPlace)
        {
            if (string.IsNullOrWhiteSpace((string)name))
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

        public PlayerName Name { get; }

        public int ArmiesToPlace { get; private set; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }

        public void ArmyPlaced()
        {
            if (ArmiesToPlace == 0)
            {
                throw new InvalidOperationException("Player can't place more armies than player has left");
            }
            ArmiesToPlace--;
        }
    }
}