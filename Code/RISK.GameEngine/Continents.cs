using System.Collections.Generic;

namespace RISK.GameEngine
{
    public class Continents : IContinents
    {
        public Continents()
        {
            Asia = new Continent(7);
            NorthAmerica = new Continent(5);
            Europe = new Continent(5);
            Africa = new Continent(3);
            Australia = new Continent(2);
            SouthAmerica = new Continent(2);
        }

        public IEnumerable<IContinent> GetAll()
        {
            return new[]
            {
                Asia,
                NorthAmerica,
                Europe,
                Africa,
                Australia,
                SouthAmerica
            };
        }

        public IContinent Asia { get; }
        public IContinent NorthAmerica { get; }
        public IContinent Europe { get; }
        public IContinent Africa { get; }
        public IContinent Australia { get; }
        public IContinent SouthAmerica { get; }
    }
}