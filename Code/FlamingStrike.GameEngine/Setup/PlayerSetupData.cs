using System;

namespace FlamingStrike.GameEngine.Setup
{
    public class PlayerSetupData
    {
        public PlayerSetupData(IPlayer player, int armiesToPlace)
        {
            if (armiesToPlace < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armiesToPlace), armiesToPlace, "value must be greater than or equal to zero");
            }
            Player = player;
            ArmiesToPlace = armiesToPlace;
        }

        public IPlayer Player { get; }

        public int ArmiesToPlace { get; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }
    }
}