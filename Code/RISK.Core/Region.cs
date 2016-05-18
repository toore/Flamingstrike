using System.Collections.Generic;

namespace RISK.Core
{
    public interface IRegion
    {
        IContinent Continent { get; }
        IReadOnlyList<IRegion> GetBorderingTerritories { get; }
        bool HasBorder(IRegion region);
    }
}