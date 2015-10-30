using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface ITerritoryId
    {
        Continent Continent { get; }
        IReadOnlyList<ITerritoryId> GetBorderingTerritories { get; }
        bool HasBorderTo(ITerritoryId territoryId);
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

        public bool HasBorderTo(ITerritoryId territoryId)
        {
            return _borderingTerritories.Contains(territoryId);
        }

        public void AddBorderToTerritories(params ITerritoryId[] territoriesId)
        {
            _borderingTerritories.AddRange(territoriesId);
        }
    }
}