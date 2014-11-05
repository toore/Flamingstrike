using System.Collections.Generic;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        private readonly Player _player;

        public LocationSelectorParameter(IEnumerable<ITerritory> allTerritories, IEnumerable<ITerritory> enabledTerritories, Player player)
        {
            AllTerritories = allTerritories;
            _player = player;
            EnabledTerritories = enabledTerritories;
        }

        public IEnumerable<ITerritory> AllTerritories { get; private set; }
        public IEnumerable<ITerritory> EnabledTerritories { get; private set; }

        public IPlayer GetPlayerThatTakesTurn()
        {
            return _player.GetPlayer();
        }

        public int GetArmiesLeft()
        {
            return _player.ArmiesToPlace;
        }

    }
}