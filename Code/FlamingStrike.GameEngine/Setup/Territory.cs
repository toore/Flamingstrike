namespace FlamingStrike.GameEngine.Setup
{
    public class Territory
    {
        public Territory(IRegion region, Player player)
        {
            Region = region;
            Player = player;
        }

        public IRegion Region { get; }
        public Player Player { get; }
        public int Armies { get; private set; }

        public void PlaceArmy()
        {
            Player.ArmyPlaced();
            Armies++;
        }
    }
}