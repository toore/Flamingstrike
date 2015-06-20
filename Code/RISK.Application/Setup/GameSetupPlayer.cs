namespace RISK.Application.Setup
{
    public class GameSetupPlayer
    {
        public int ArmiesToPlace { get; set; }

        public GameSetupPlayer(IPlayer player, int armiesToPlace)
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