using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestParameter
    {
        IReadOnlyList<Play.ITerritory> Territories { get; }
        IReadOnlyList<ITerritoryId> EnabledTerritories { get; }
        IPlayer Player { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly InSetupPlayer _inSetupPlayer;

        public TerritoryRequestParameter(IReadOnlyList<Play.Territory> territories, IReadOnlyList<ITerritoryId> enabledTerritories, InSetupPlayer inSetupPlayer)
        {
            Territories = territories;
            _inSetupPlayer = inSetupPlayer;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<Play.ITerritory> Territories { get; }
        public IReadOnlyList<ITerritoryId> EnabledTerritories { get; }
        public IPlayer Player => _inSetupPlayer.Player;

        public int GetArmiesLeftToPlace()
        {
            return _inSetupPlayer.ArmiesToPlace;
        }
    }
}