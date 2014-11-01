using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class Location : ILocation
    {
        private readonly List<ILocation> _borders;

        public Location(string name, Continent continent)
        {
            Name = name;
            Continent = continent;
            _borders = new List<ILocation>();
        }

        public string Name { get; private set; }
        public Continent Continent { get; private set; }

        public bool IsBordering(ILocation location)
        {
            return _borders.Contains(location);
        }

        public void AddBorders(params ILocation[] locations)
        {
            _borders.AddRange(locations);
        }
    }
}