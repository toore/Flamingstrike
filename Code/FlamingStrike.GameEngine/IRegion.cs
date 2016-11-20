using System.Collections.Generic;

namespace FlamingStrike.GameEngine
{
    public interface IRegion
    {
        IContinent Continent { get; }
        bool HasBorder(IRegion region);
        IEnumerable<IRegion> GetBorderingRegions();
    }
}