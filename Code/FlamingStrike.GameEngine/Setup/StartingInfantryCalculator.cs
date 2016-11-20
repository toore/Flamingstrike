namespace FlamingStrike.GameEngine.Setup
{
    public interface IStartingInfantryCalculator
    {
        int Get(int numberOfPlayers);
    }

    public class StartingInfantryCalculator : IStartingInfantryCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return numberOfPlayers * -5 + 50;
        }
    }
}