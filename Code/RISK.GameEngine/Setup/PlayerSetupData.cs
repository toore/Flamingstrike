using System;

namespace RISK.GameEngine.Setup
{
    public class PlayerSetupData
    {
        public PlayerSetupData(IPlayer player)
        {
            Player = player;
        }

        public IPlayer Player { get; private set; }

        public int ArmiesToPlace { get; private set; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }

        public void SetArmiesToPlace(int armiesToPlace)
        {
            if (armiesToPlace < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(armiesToPlace), armiesToPlace, "value must be greater than or equal to zero");
            }
            ArmiesToPlace = armiesToPlace;
        }

        public void PlaceArmy(Territory territory)
        {
            if (!HasArmiesLeftToPlace())
            {
                throw new InvalidOperationException("No armies left to place");
            }
            territory.Armies++;
            ArmiesToPlace--;
        }
    }
}