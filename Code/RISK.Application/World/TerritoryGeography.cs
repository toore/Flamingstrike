using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface ITerritoryGeography
    {
        Continent Continent { get; }
        IReadOnlyList<ITerritoryGeography> GetBorderingTerritories { get; }
        bool HasBorder(ITerritoryGeography territoryGeography);
    }

    public class TerritoryGeography : ITerritoryGeography
    {
        private readonly List<ITerritoryGeography> _borderingTerritories;

        public TerritoryGeography(Continent continent)
        {
            Continent = continent;
            _borderingTerritories = new List<ITerritoryGeography>();
        }

        public Continent Continent { get; }
        public IReadOnlyList<ITerritoryGeography> GetBorderingTerritories => _borderingTerritories;

        public bool HasBorder(ITerritoryGeography territoryGeography)
        {
            return _borderingTerritories.Contains(territoryGeography);
        }

        public void AddBorders(params ITerritoryGeography[] territoriesGeography)
        {
            _borderingTerritories.AddRange(territoriesGeography);
        }
    }
}