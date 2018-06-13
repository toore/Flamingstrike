using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class SelectRegion
    {
        public SelectRegion(string player, int armiesLeftToPlace, IReadOnlyList<Territory> territories)
        {
            Player = player;
            ArmiesLeftToPlace = armiesLeftToPlace;
            Territories = territories;
        }

        public string Player { get; }
        
        public int ArmiesLeftToPlace { get; }
        
        public IReadOnlyList<Territory> Territories { get; }
    }
}