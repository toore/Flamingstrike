namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished
{
    public class Territory
    {
        public Territory(Region region, string player, int armies)
        {
            Region = region;
            Player = player;
            Armies = armies;
        }

        public Region Region { get; }
        public string Player { get; }
        public int Armies { get; }
    }
}