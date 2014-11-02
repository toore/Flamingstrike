namespace RISK.Application
{
    public class Continent
    {
        public static readonly Continent NorthAmerica = new Continent(5);
        public static readonly Continent SouthAmerica = new Continent(2);
        public static readonly Continent Europe = new Continent(5);
        public static readonly Continent Africa = new Continent(3);
        public static readonly Continent Asia = new Continent(7);
        public static readonly Continent Australia = new Continent(2);

        private Continent(int bonus)
        {
            Bonus = bonus;
        }

        public int Bonus { get; private set; }
    }
}