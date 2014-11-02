using System.Collections.Generic;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public class LocationSelectorParameter : ILocationSelectorParameter
    {
        private readonly PlayerDuringGameSetup _playerDuringGameSetup;

        public LocationSelectorParameter(IEnumerable<ITerritory> allTerritories, IEnumerable<ITerritory> enabledTerritories, PlayerDuringGameSetup playerDuringGameSetup)
        {
            AllTerritories = allTerritories;
            _playerDuringGameSetup = playerDuringGameSetup;
            EnabledTerritories = enabledTerritories;
        }

        public IEnumerable<ITerritory> AllTerritories { get; private set; }
        public IEnumerable<ITerritory> EnabledTerritories { get; private set; }

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