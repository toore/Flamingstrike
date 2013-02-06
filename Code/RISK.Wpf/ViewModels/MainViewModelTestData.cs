using System.Linq;
using GuiWpf.Services;
using GuiWpf.Views.Main;
using GuiWpf.Views.WorldMapView;
using GuiWpf.Views.WorldMapView.Territories;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainViewModelTestData : MainViewModel
    {
        public MainViewModelTestData() : base(new GameEngineStub()) {}

        private class GameEngineStub : IGameEngine
        {
            public WorldMapViewModel GetWorldMapViewModel()
            {
                var continentRepository = new ContinentRepository();
                var locationRepository = new LocationRepository(continentRepository);
                var colorService = new ColorService();
                var territoryViewModelsFactorySelector = new TerritoryViewModelsFactorySelector(locationRepository, colorService);

                var worldMap = new WorldMap(locationRepository);
                var territory = worldMap.GetTerritory(locationRepository.Brazil);
                territory.Owner = new HumanPlayer("pelle");
                territory.Armies = 99;

                var worldMapViewModels = new WorldMapViewModelFactory(territoryViewModelsFactorySelector, locationRepository, colorService).Create(worldMap, null).WorldMapViewModels.ToList();

                return new WorldMapViewModel
                    {
                        WorldMapViewModels = worldMapViewModels
                    };
            }
        }
    }
}