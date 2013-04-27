using RISK.Domain.Entities;

namespace RISK.Domain.Repositories
{
    public class ContinentProvider : IContinentProvider
    {
        public ContinentProvider()
        {
            Australia = new Continent { Bonus = 2 };
            Asia = new Continent { Bonus = 7 };
            Africa = new Continent { Bonus = 3 };
            Europe = new Continent { Bonus = 5 };
            SouthAmerica = new Continent { Bonus = 2 };
            NorthAmerica = new Continent { Bonus = 5 };
        }

        public Continent NorthAmerica { get; private set; }
        public Continent SouthAmerica { get; private set; }
        public Continent Europe { get; private set; }
        public Continent Africa { get; private set; }
        public Continent Asia { get; private set; }
        public Continent Australia { get; private set; }
    }
}