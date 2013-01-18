using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class AreaDefinition : IAreaDefinition
    {
        private readonly List<AreaDefinition> _neighbors;

        public AreaDefinition(string name, Continent continent)
        {
            TranslationKey = name;
            Continent = continent;
            _neighbors = new List<AreaDefinition>();
        }

        public string TranslationKey { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<IAreaDefinition> Neighbors
        {
            get { return _neighbors; }
        }

        public void AddNeighbors(params AreaDefinition[] neighbors)
        {
            _neighbors.AddRange(neighbors);
        }
    }
}