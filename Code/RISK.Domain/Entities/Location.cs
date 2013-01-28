using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class Location : ILocation
    {
        private readonly List<Location> _connections;

        public Location(string name, Continent continent)
        {
            TranslationKey = name;
            Continent = continent;
            _connections = new List<Location>();
        }

        public string TranslationKey { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<ILocation> Connections
        {
            get { return _connections; }
        }

        public void AddConnectedTerritories(params Location[] territories)
        {
            _connections.AddRange(territories);
        }
    }
}