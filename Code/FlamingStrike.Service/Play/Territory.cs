namespace FlamingStrike.Service.Play
{
    public class Territory
    {
        public Territory(string region, string playerName, int armies)
        {
            Region = region;
            PlayerName = playerName;
            Armies = armies;
        }

        public string Region { get; }
        public string PlayerName { get; }
        public int Armies { get; }
    }
}