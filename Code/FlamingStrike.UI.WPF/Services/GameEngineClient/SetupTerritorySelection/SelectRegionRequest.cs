using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection
{
    public class SelectRegionRequest
    {
        public string Player { get; set; }
        public int ArmiesLeftToPlace { get; set; }
        public IReadOnlyList<Territory> Territories { get; set; }
    }
}