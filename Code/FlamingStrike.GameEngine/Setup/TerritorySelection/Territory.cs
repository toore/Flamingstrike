namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public class Territory
    {
        public Territory(IRegion region, PlayerName name, int armies, bool isSelectable)
        {
            Region = region;
            Name = name;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public IRegion Region { get; }
        public PlayerName Name { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}