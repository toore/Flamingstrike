using System.Collections.Generic;
using RISK.Domain;
using RISK.Domain.Entities;

namespace RISK.Tests
{
    public class LocationBuilder
    {
        private string _name = "";
        private Continent _continent = Continent.Europe;
        private readonly List<ILocation> _borders = new List<ILocation>();

        public Location Build()
        {
            var location = new Location(_name, _continent);
            location.AddBorders(_borders.ToArray());
            return location;
        }

        public LocationBuilder WithBorder(ILocation location)
        {
            _borders.Add(location);
            return this;
        }
    }
}