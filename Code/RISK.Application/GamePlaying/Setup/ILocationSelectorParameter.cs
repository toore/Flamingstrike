using System.Collections.Generic;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface ILocationSelectorParameter
    {
        IEnumerable<ITerritory> AllTerritories { get; }
        IEnumerable<ITerritory> EnabledTerritories { get; }
        IPlayer GetPlayerThatTakesTurn();
        int GetArmiesLeft();
    }
}