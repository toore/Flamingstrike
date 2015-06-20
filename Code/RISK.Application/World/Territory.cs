using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface ITerritory
    {
        Continent Continent { get; }
        bool HasBorderTo(ITerritory territory);
    }

    public class Territory : ITerritory
    {
        private readonly List<ITerritory> _borders;

        public Territory(string name, Continent continent)
        {
            Continent = continent;
            _borders = new List<ITerritory>();
        }

        public Continent Continent { get; }

        public bool HasBorderTo(ITerritory territory)
        {
            return _borders.Contains(territory);
        }

        public void AddBorderToTerritories(params ITerritory[] locations)
        {
            _borders.AddRange(locations);
        }
    }
}