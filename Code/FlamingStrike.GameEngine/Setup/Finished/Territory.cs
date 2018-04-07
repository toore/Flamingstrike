namespace FlamingStrike.GameEngine.Setup.Finished
{
    public class Territory
    {
        public IRegion Region { get; }
        public string PlayerName { get; }
        public int Armies { get; }

        public Territory(IRegion region, string playerName, int armies)
        {
            Region = region;
            PlayerName = playerName;
            Armies = armies;
        }
    }
}