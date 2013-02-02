using GuiWpf.Services;
using GuiWpf.Views.WorldMapView.Territories;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapView
{
    public class WorldMapViewModelTestData : WorldMapViewModel
    {
        public WorldMapViewModelTestData()
        {
            var continentRepository = new ContinentRepository();
            var locationRepository = new LocationRepository(continentRepository);
            var colorService = new ColorService();
            var territoryViewModelFactory = new TerritoryViewModelFactory(locationRepository, continentRepository, colorService);
            var worldMap = new WorldMap(locationRepository);

            Territories = new TerritoryViewModelsFactory(territoryViewModelFactory, worldMap, locationRepository, colorService).Create();
        }
    }
}