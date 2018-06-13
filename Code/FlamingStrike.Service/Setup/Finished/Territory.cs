namespace FlamingStrike.Service.Setup.Finished
{
    public class Territory
    {
        public Territory(string region, string player, int armies)
        {
            Region = region;
            Player = player;
            Armies = armies;
        }

        public string Region { get; }
        public string Player { get; }
        public int Armies { get; }
    }
}