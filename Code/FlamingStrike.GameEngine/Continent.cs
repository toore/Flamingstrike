namespace FlamingStrike.GameEngine
{
    public class Continent : IContinent
    {
        public Continent(int bonus)
        {
            Bonus = bonus;
        }

        public int Bonus { get; }
    }
}