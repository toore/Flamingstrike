using System.Linq;
using Caliburn.Micro;
using GuiWpf.GuiDefinitions;
using GuiWpf.Services;
using GuiWpf.ViewModels.WorldMapViewModels;
using GuiWpf.Views.WorldMap;
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
            private WorldMapViewModel CreateWorldMapViewModel()
            {
                var continentRepository = new ContinentRepository();
                var locationRepository = new LocationRepository(continentRepository);
                var colorService = new ColorService();
                var territoryColorsFactory = new TerritoryColorsFactory(locationRepository, colorService);
                var territoryLayoutInformationFactory = new TerritoryLayoutInformationFactory(locationRepository);
                var territoryViewModelFactory = new TerritoryViewModelFactory(territoryColorsFactory, territoryLayoutInformationFactory);

                var worldMap = new WorldMap(locationRepository);
                var territory = worldMap.GetTerritory(locationRepository.Brazil);
                territory.Owner = new HumanPlayer("pelle");
                territory.Armies = 99;

                var textViewModelFactory = new TextViewModelFactory(territoryLayoutInformationFactory);
                var worldMapViewModelFactory = new WorldMapViewModelFactory(locationRepository, territoryViewModelFactory, textViewModelFactory);

                var worldMapViewModels = worldMapViewModelFactory.Create(worldMap, null).WorldMapViewModels.ToList();

                var worldMapViewModel = new WorldMapViewModel();
                worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

                return worldMapViewModel;
            }

            WorldMapViewModel IGameEngine.WorldMapViewModel
            {
                get { return CreateWorldMapViewModel(); }
            }
        }
    }
}