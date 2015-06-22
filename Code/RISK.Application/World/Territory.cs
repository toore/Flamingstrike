using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface ITerritory
    {
        Continent Continent { get; }
        IReadOnlyList<ITerritory> GetBorderingTerritories { get; }
        bool HasBorderTo(ITerritory territory);
    }

    public class Territory : ITerritory
    {
        private readonly List<ITerritory> _borderingTerritories;

        public Territory(string name, Continent continent)
        {
            Continent = continent;
            _borderingTerritories = new List<ITerritory>();
        }

        public Continent Continent { get; }
        public IReadOnlyList<ITerritory> GetBorderingTerritories => _borderingTerritories;

        public bool HasBorderTo(ITerritory territory)
        {
            return _borderingTerritories.Contains(territory);
        }

        public void AddBorderToTerritories(params ITerritory[] territories)
        {
            _borderingTerritories.AddRange(territories);
        }
    }
}