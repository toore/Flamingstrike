using System.Collections.Generic;
using RISK.WorldMap.Territories;

namespace RISK.WorldMap
{
    public class WorldMapViewModel
    {
        public IEnumerable<TerritoryViewModelBase> Territories { get; set; }
    }
}