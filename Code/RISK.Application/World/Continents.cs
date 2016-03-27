using System.Collections;
using System.Collections.Generic;

namespace RISK.Application.World
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

    public interface IContinents : IEnumerable<IContinent>
    {
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

        public IContinent Asia { get; }
        public IContinent NorthAmerica { get; }
        public IContinent Europe { get; }
        public IContinent Africa { get; }
        public IContinent Australia { get; }
        public IContinent SouthAmerica { get; }

        public IEnumerator<IContinent> GetEnumerator()
        {
            return new List<IContinent>
            {
                Asia,
                NorthAmerica,
                Europe,
                Africa,
                Australia,
                SouthAmerica
            }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}