namespace RISK.Application.GamePlaying.Setup
{
    public interface IInitialArmyAssignmentCalculator
    {
        int Get(int numberOfPlayers);
    }

    public class InitialArmyAssignmentCalculator : IInitialArmyAssignmentCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}