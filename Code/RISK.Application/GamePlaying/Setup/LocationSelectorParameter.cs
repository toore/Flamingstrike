using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        private readonly PlayerDuringGameSetup _playerDuringGameSetup;

        public LocationSelectorParameter(IWorldMap worldMap, IEnumerable<ILocation> availableLocations, PlayerDuringGameSetup playerDuringGameSetup)
        {
            _playerDuringGameSetup = playerDuringGameSetup;
            WorldMap = worldMap;
            AvailableLocations = availableLocations;

        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ILocation> AvailableLocations { get; private set; }
        public IPlayer GetPlayerThatTakesTurn()
        {
            return _playerDuringGameSetup.GetPlayer();
        }

        public int GetArmiesLeft()
        {
            return _playerDuringGameSetup.GetArmiesLeft();
        }

    }
}