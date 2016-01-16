namespace RISK.Application.Setup
{
    public class InSetupPlayer
    {
        public int ArmiesToPlace { get; set; }

        public InSetupPlayer(IPlayer player, int armiesToPlace)
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