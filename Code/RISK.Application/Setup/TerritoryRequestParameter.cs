using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestParameter
    {
        IReadOnlyList<Play.ITerritory> Territories { get; }
        IReadOnlyList<ITerritoryId> EnabledTerritories { get; }
        IPlayerId PlayerId { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly Player _player;

        public TerritoryRequestParameter(IReadOnlyList<Play.Territory> territories, IReadOnlyList<ITerritoryId> enabledTerritories, Player player)
        {
            Territories = territories;
            _player = player;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<Play.ITerritory> Territories { get; }
        public IReadOnlyList<ITerritoryId> EnabledTerritories { get; }
        public IPlayerId PlayerId => _player.PlayerId;

        public int GetArmiesLeftToPlace()
        {
            return _player.ArmiesToPlace;
        }
    }
}