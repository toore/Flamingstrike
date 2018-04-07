namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public class Player
    {
        public string Name { get; }
        public int ArmiesToPlace { get; }

        public Player(string name, int armiesToPlace)
        {
            Name = name;
            ArmiesToPlace = armiesToPlace;
        }
    }
}