using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        public LocationSelectorParameter(IWorldMap worldMap, IEnumerable<ILocation> availableLocations, PlayerDuringSetup playerDuringSetup)
        {
            WorldMap = worldMap;
            AvailableLocations = availableLocations;
            PlayerDuringSetup = playerDuringSetup;
        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ILocation> AvailableLocations { get; private set; }
        public PlayerDuringSetup PlayerDuringSetup { get; set; }
    }
}