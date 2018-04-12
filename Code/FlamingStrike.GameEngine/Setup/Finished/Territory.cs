namespace FlamingStrike.GameEngine.Setup.Finished
{
    public class Territory
    {
        public Territory(Region region, PlayerName name, int armies)
        {
            Region = region;
            Name = name;
            Armies = armies;
        }

        public Region Region { get; }
        public PlayerName Name { get; }
        public int Armies { get; }
    }
}