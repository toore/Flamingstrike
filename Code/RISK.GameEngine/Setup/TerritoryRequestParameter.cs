using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Setup
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
        public TerritoryRequestParameter(IReadOnlyList<Territory> territories, IReadOnlyList<IRegion> enabledTerritories, IPlayer player)
        {
            Territories = territories;
            Player = player;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IRegion> EnabledTerritories { get; }
        public IPlayer Player { get; }

        public int GetArmiesLeftToPlace()
        {
            return Player.ArmiesToPlace;
        }
    }
}