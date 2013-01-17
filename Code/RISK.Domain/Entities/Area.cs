using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public class Area : IArea
    {
        private readonly List<Area> _neighbors;

        public Area(string name, Continent continent)
        {
            TranslationKey = name;
            Continent = continent;
            _neighbors = new List<Area>();
        }

        public string TranslationKey { get; private set; }
        public Continent Continent { get; private set; }

        public IEnumerable<IArea> Neighbors
        {
            get { return _neighbors; }
        }

        public void AddNeighbors(params Area[] neighbors)
        {
            _neighbors.AddRange(neighbors);
        }
    }
}