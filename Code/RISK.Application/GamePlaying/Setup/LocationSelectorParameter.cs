using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        public LocationSelectorParameter(IWorldMap worldMap, IEnumerable<ILocation> availableLocations, SetupArmies setupArmies)
        {
            WorldMap = worldMap;
            AvailableLocations = availableLocations;
            SetupArmies = setupArmies;
        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ILocation> AvailableLocations { get; private set; }
        public SetupArmies SetupArmies { get; private set; }
    }
}