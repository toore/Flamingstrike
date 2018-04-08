namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public class Player
    {
        public PlayerName Name { get; }
        public int ArmiesToPlace { get; }

        public Player(PlayerName name, int armiesToPlace)
        {
            Name = name;
            ArmiesToPlace = armiesToPlace;
        }
    }
}