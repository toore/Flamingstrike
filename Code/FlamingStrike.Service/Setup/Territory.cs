namespace FlamingStrike.Service.Setup
{
    public class Territory
    {
        public Territory(string region, string player, int armies, bool isSelectable)
        {
            Region = region;
            Player = player;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public string Region { get; }
        public string Player { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}