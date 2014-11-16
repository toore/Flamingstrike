using System.Collections.Generic;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying.Setup
{
    public interface ILocationSelectorParameter
    {
        IEnumerable<ITerritory> EnabledTerritories { get; }
        IWorldMap WorldMap { get; }
        IPlayer GetPlayerThatTakesTurn();
        int GetArmiesLeft();
    }
}