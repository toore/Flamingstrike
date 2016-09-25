using System.Collections.Generic;

namespace RISK.Core
{
    public interface IRegion
    {
        IContinent Continent { get; }
        bool HasBorder(IRegion region);
        IEnumerable<IRegion> GetBorderingRegions();
    }
}