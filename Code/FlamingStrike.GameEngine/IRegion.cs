using System.Collections.Generic;

namespace FlamingStrike.GameEngine
{
    public interface IRegion
    {
        Continent Continent { get; }
        bool HasBorder(IRegion region);
        IEnumerable<IRegion> GetBorderingRegions();
    }
}