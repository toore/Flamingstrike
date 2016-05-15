using System.Collections.Generic;

namespace RISK.Core
{
    public interface IContinent
    {
        int Bonus { get; }
    }

    public class Continent : IContinent
    {
        public Continent(int bonus)
        {
            Bonus = bonus;
        }

        public int Bonus { get; }
    }

    public interface IContinents
    {
        IEnumerable<IContinent> GetAll();

        IContinent Asia { get; }
        IContinent NorthAmerica { get; }
        IContinent Europe { get; }
        IContinent Africa { get; }
        IContinent Australia { get; }
        IContinent SouthAmerica { get; }
    }

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