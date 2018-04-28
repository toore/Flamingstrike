namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection
{
    public class Territory
    {
        public Territory(Region region, string name, int armies, bool isSelectable)
        {
            Region = region;
            Name = name;
            Armies = armies;
            IsSelectable = isSelectable;
        }

        public Region Region { get; }
        public string Name { get; }
        public int Armies { get; }
        public bool IsSelectable { get; }
    }
}