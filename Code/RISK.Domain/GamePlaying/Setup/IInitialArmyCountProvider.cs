namespace RISK.Domain.GamePlaying.Setup
{
    public interface IInitialArmyCountProvider
    {
        int Get(int numberOfPlayers);
    }
}