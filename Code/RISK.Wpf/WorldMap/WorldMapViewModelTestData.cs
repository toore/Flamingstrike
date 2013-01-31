using RISK.WorldMap.Territories;

namespace RISK.WorldMap
{
    public class WorldMapViewModelTestData : WorldMapViewModel
    {
        public WorldMapViewModelTestData()
        {
            Territories = new TerritoryViewModelBase[]
                {
                    new AlaskaViewModel(),
                    new CentralAmericaViewModel()
                };
        }
    }
}