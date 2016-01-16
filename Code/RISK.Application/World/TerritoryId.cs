using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface ITerritoryId
    {
        Continent Continent { get; }
        IReadOnlyList<ITerritoryId> GetBorderingTerritories { get; }
        bool HasBorder(ITerritoryId territoryId);
    }

    public class TerritoryId : ITerritoryId
    {
        private readonly List<ITerritoryId> _borderingTerritories;

        public TerritoryId(string name, Continent continent)
        {
            Continent = continent;
            _borderingTerritories = new List<ITerritoryId>();
        }

        public Continent Continent { get; }
        public IReadOnlyList<ITerritoryId> GetBorderingTerritories => _borderingTerritories;

        public bool HasBorder(ITerritoryId territoryId)
        {
            return _borderingTerritories.Contains(territoryId);
        }

        public void AddBorders(params ITerritoryId[] territoriesId)
        {
            _borderingTerritories.AddRange(territoriesId);
        }
    }
}