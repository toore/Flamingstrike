using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class Location : ILocation
    {
        private readonly List<Location> _borders;

        public Location(string name, Continent continent)
        {
            Name = name;
            Continent = continent;
            _borders = new List<Location>();
        }

        public string Name { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<ILocation> Borders
        {
            get { return _borders; }
        }

        public void AddBorderingTerritories(params Location[] territories)
        {
            _borders.AddRange(territories);
        }
    }
}