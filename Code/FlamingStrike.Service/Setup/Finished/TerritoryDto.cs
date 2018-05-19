namespace FlamingStrike.Service.Setup.Finished
{
    public class TerritoryDto
    {
        public TerritoryDto(string region, string player, int armies)
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