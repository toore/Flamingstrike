namespace RISK.Domain.GamePlaying.Setup
{
    public interface IInitialArmyCount
    {
        int Get(int numberOfPlayers);
    }
}