namespace RISK.Application.Setup
{
    public class Player
    {
        public int ArmiesToPlace { get; set; }

        public Player(IPlayerId playerId, int armiesToPlace)
        {
            PlayerId = playerId;
            ArmiesToPlace = armiesToPlace;
        }

        public IPlayerId PlayerId { get; }

        public bool HasArmiesLeftToPlace()
        {
            return ArmiesToPlace > 0;
        }
    }
}