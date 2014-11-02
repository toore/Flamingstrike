using System.Collections.Generic;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public interface ILocationSelectorParameter
    {
        IEnumerable<ITerritory> AllTerritories { get; }
        IEnumerable<ITerritory> EnabledTerritories { get; }
        IPlayer GetPlayerThatTakesTurn();
        int GetArmiesLeft();
    }
}