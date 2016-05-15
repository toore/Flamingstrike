using System.Collections.Generic;
using RISK.Core;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestParameter
    {
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IRegion> EnabledTerritories { get; }
        IPlayer Player { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly InSetupPlayer _inSetupPlayer;

        public TerritoryRequestParameter(IReadOnlyList<Territory> territories, IReadOnlyList<IRegion> enabledTerritories, InSetupPlayer inSetupPlayer)
        {
            Territories = territories;
            _inSetupPlayer = inSetupPlayer;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IRegion> EnabledTerritories { get; }
        public IPlayer Player => _inSetupPlayer.Player;

        public int GetArmiesLeftToPlace()
        {
            return _inSetupPlayer.ArmiesToPlace;
        }
    }
}