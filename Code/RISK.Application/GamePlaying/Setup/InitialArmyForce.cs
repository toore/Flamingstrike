namespace RISK.Application.GamePlaying.Setup
{
    public interface IInitialArmyForce
    {
        int Get(int numberOfPlayers);
    }

    public class InitialArmyForce : IInitialArmyForce
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}