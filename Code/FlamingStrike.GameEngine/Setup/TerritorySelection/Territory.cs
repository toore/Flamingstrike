namespace FlamingStrike.GameEngine.Setup.TerritorySelection
{
    public class Territory
    {
        public Territory(Region region, PlayerName name, int armies, bool isSelectable)
        {
            Region = region;
            Name = name;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public Region Region { get; }
        public PlayerName Name { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}