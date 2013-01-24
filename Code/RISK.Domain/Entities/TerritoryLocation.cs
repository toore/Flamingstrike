using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class TerritoryLocation : ITerritoryLocation
    {
        private readonly List<TerritoryLocation> _connectedTerritories;

        public TerritoryLocation(string name, Continent continent)
        {
            TranslationKey = name;
            Continent = continent;
            _connectedTerritories = new List<TerritoryLocation>();
        }

        public string TranslationKey { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<ITerritoryLocation> ConnectedTerritories
        {
            get { return _connectedTerritories; }
        }

        public void AddConnectedTerritories(params TerritoryLocation[] territories)
        {
            _connectedTerritories.AddRange(territories);
        }
    }
}