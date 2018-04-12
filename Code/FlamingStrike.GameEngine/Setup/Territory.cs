namespace FlamingStrike.GameEngine.Setup
{
    public class Territory
    {
        public Territory(Region region, Player player)
        {
            Region = region;
            Player = player;
        }

        public Region Region { get; }
        public Player Player { get; }
        public int Armies { get; private set; }

        public void PlaceArmy()
        {
            Player.ArmyPlaced();
            Armies++;
        }
    }
}