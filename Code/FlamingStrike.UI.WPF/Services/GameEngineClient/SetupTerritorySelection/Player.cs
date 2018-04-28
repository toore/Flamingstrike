namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection
{
    public class Player
    {
        public string Name { get; }
        public int ArmiesToPlace { get; }

        public Player(string name, int armiesToPlace)
        {
            Name = name;
            ArmiesToPlace = armiesToPlace;
        }
    }
}