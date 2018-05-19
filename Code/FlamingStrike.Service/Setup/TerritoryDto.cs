namespace FlamingStrike.Service.Setup
{
    public class TerritoryDto
    {
        public TerritoryDto(string region, string player, int armies, bool isSelectable)
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