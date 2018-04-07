namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public class Territory
    {
        public Territory(IRegion region, string player, int armies, bool isSelectable)
        {
            Region = region;
            Player = player;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public IRegion Region { get; }
        public string Player { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}