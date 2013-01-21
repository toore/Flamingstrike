using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class AreaDefinition : IAreaDefinition
    {
        private readonly List<AreaDefinition> _connectedAreas;

        public AreaDefinition(string name, Continent continent)
        {
            TranslationKey = name;
            Continent = continent;
            _connectedAreas = new List<AreaDefinition>();
        }

        public string TranslationKey { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<IAreaDefinition> ConnectedAreas
        {
            get { return _connectedAreas; }
        }

        public void AddConnectedAreas(params AreaDefinition[] connectedAreas)
        {
            _connectedAreas.AddRange(connectedAreas);
        }
    }
}