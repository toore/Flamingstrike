namespace FlamingStrike.Service.Play
{
    public class TerritoryDto
    {
        public TerritoryDto(string region, string playerName, int armies)
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