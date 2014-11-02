namespace RISK.Application.GamePlaying.Setup
{
    public class InitialArmyCount : IInitialArmyCount
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}