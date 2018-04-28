namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public class Territory
    {
        public Territory(Region region, string playerName, int armies)
        {
            Region = region;
            PlayerName = playerName;
            Armies = armies;
        }

        public Region Region { get; }
        public string PlayerName { get; }
        public int Armies { get; }

        public int GetMaxNumberOfPossibleAttackingArmies()
        {
            return Armies - 1;
        }
    }
}