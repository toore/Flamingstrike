using RISK.Domain.Extensions;
using RISK.WorldMap.Territories;

namespace RISK.WorldMap
{
    public class WorldMapViewModelTestData : WorldMapViewModel
    {
        public WorldMapViewModelTestData()
        {
            Territories = new AlaskaTerritoryViewModel().AsList();
        }
    }
}