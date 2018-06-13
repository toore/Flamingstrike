using System.Collections.Generic;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Proxy
{
    public class SelectRegion
    {
        public string Player { get; set; }
        public int ArmiesLeftToPlace { get; set; }
        public IReadOnlyList<Territory> Territories { get; set; }
    }
}