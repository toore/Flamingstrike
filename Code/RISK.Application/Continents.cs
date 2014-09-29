using RISK.Domain.Entities;

namespace RISK.Domain
{
    public static class Continents
    {
        public static readonly Continent NorthAmerica = new Continent { Bonus = 5 };
        public static readonly Continent SouthAmerica = new Continent { Bonus = 2 };
        public static readonly Continent Europe = new Continent { Bonus = 5 };
        public static readonly Continent Africa = new Continent { Bonus = 3 };
        public static readonly Continent Asia = new Continent { Bonus = 7 };
        public static readonly Continent Australia = new Continent { Bonus = 2 };
    }
}