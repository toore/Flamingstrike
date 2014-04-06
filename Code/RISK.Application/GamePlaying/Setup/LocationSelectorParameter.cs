using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        public LocationSelectorParameter(IWorldMap worldMap, IEnumerable<ILocation> availableLocations, SetupPlayer setupPlayer)
        {
            WorldMap = worldMap;
            AvailableLocations = availableLocations;
            SetupPlayer = setupPlayer;
        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ILocation> AvailableLocations { get; private set; }
        public SetupPlayer SetupPlayer { get; private set; }
    }
}