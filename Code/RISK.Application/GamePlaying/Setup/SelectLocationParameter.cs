using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class SelectLocationParameter : ISelectLocationParameter
    {
        public SelectLocationParameter(IWorldMap worldMap, IEnumerable<ILocation> availableLocations)
        {
            WorldMap = worldMap;
            AvailableLocations = availableLocations;
        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ILocation> AvailableLocations { get; private set; }
    }
}