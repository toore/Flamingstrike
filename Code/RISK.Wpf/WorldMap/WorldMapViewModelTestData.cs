using RISK.Services;
using RISK.WorldMap.Territories;

namespace RISK.WorldMap
{
    public class WorldMapViewModelTestData : WorldMapViewModel
    {
        public WorldMapViewModelTestData()
        {
            Territories = new TerritoryViewModelFactory(new ColorService()).Create();

        }
    }
}