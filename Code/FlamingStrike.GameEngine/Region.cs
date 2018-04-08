using System.Collections.Generic;

namespace FlamingStrike.GameEngine
{
    public class Region : IRegion
    {
        private readonly List<IRegion> _regionsWithBorderToThisRegion;

        public Region(Continent continent)
        {
            Continent = continent;
            _regionsWithBorderToThisRegion = new List<IRegion>();
        }

        public Continent Continent { get; }

        public bool HasBorder(IRegion region)
        {
            return _regionsWithBorderToThisRegion.Contains(region);
        }

        public IEnumerable<IRegion> GetBorderingRegions()
        {
            return _regionsWithBorderToThisRegion;
        }

        public void AddBordersToRegions(params IRegion[] regions)
        {
            _regionsWithBorderToThisRegion.AddRange(regions);
        }
    }
}