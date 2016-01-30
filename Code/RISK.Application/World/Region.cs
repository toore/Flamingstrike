using System.Collections.Generic;

namespace RISK.Application.World
{
    public interface IRegion
    {
        Continent Continent { get; }
        IReadOnlyList<IRegion> GetBorderingTerritories { get; }
        bool HasBorder(IRegion region);
    }

    public class Region : IRegion
    {
        private readonly List<IRegion> _borderingTerritories;

        public Region(Continent continent)
        {
            Continent = continent;
            _borderingTerritories = new List<IRegion>();
        }

        public Continent Continent { get; }
        public IReadOnlyList<IRegion> GetBorderingTerritories => _borderingTerritories;

        public bool HasBorder(IRegion region)
        {
            return _borderingTerritories.Contains(region);
        }

        public void AddBorders(params IRegion[] territoriesGeography)
        {
            _borderingTerritories.AddRange(territoriesGeography);
        }
    }
}