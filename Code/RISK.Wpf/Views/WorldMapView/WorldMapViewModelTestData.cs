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
            var worldMapEntityFactorySelector = new TerritoryViewModelsFactorySelector(locationRepository, colorService);
            var worldMap = new WorldMap(locationRepository);

            WorldMapViewModels = new WorldMapViewModelsFactory(worldMapEntityFactorySelector, worldMap, locationRepository).Create();
        }
    }
}