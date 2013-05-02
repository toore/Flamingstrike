using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface ISelectLocationParameter
    {
        IWorldMap WorldMap { get; }
        IEnumerable<ILocation> AvailableLocations { get; }
    }
}