namespace FlamingStrike.GameEngine.Setup.Finished
{
    public class Territory
    {
        public Territory(IRegion region, PlayerName name, int armies)
        {
            Region = region;
            Name = name;
            Armies = armies;
        }

        public IRegion Region { get; }
        public PlayerName Name { get; }
        public int Armies { get; }
    }
}