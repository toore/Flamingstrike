using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestParameter
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<ITerritoryGeography> EnabledTerritories { get; }
        IPlayer Player { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly InSetupPlayer _inSetupPlayer;

        public TerritoryRequestParameter(IReadOnlyList<Territory> territories, IReadOnlyList<ITerritoryGeography> enabledTerritories, InSetupPlayer inSetupPlayer)
        {
            Territories = territories;
            _inSetupPlayer = inSetupPlayer;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<ITerritoryGeography> EnabledTerritories { get; }
        public IPlayer Player => _inSetupPlayer.Player;

        public int GetArmiesLeftToPlace()
        {
            return _inSetupPlayer.ArmiesToPlace;
        }
    }
}