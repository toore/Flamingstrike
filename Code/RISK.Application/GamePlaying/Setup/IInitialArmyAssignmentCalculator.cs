namespace RISK.Application.GamePlaying.Setup
{
    public interface IInitialArmyAssignmentCalculator
    {
        int Get(int numberOfPlayers);
    }
}