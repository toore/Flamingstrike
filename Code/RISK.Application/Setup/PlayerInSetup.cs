namespace RISK.Application.Setup
{
    public class PlayerInSetup
    {
        public int ArmiesToPlace { get; set; }

        public PlayerInSetup(IPlayer player, int armiesToPlace)
        {
            Player = player;
            ArmiesToPlace = armiesToPlace;
        }

        public IPlayer Player { get; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }
    }
}