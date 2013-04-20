using GuiWpf.GuiDefinitions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelTestData
    {
        public static GameboardViewModel ViewModel
        {
            get { return new GameboardViewModelTestData().Create(); }
        }

        private GameboardViewModel Create()
        {
            var continentProvider = new ContinentProvider();
            var locationProvider = new LocationProvider(continentProvider);
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(locationProvider, colorService);
            var territoryLayoutInformationFactory = new TerritoryGuiDefinitionFactory(locationProvider);
            var territoryViewModelUpdater = new TerritoryViewModelUpdater(territoryColorsFactory);
            var territoryViewModelFactory = new TerritoryViewModelFactory(territoryViewModelUpdater, territoryLayoutInformationFactory);

            var worldMap = new WorldMap(locationProvider);
            var territory = worldMap.GetTerritory(locationProvider.Brazil);
            var humanPlayer = new HumanPlayer("pelle");
            territory.Owner = humanPlayer;
            territory.Armies = 99;

            var textViewModelFactory = new TextViewModelFactory(territoryLayoutInformationFactory);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(locationProvider, territoryViewModelFactory, textViewModelFactory);

            //var worldMapViewModels = worldMapViewModelFactory.Create(worldMap, null).WorldMapViewModels.ToList();

            var playerRepository = new PlayerRepository();
            playerRepository.Add(humanPlayer);

            var game = new Game(null, playerRepository, new AlternateGameSetup(playerRepository, locationProvider, new RandomOrderer(new RandomWrapper()), new WorldMapFactory(locationProvider)));
            var gameboardViewModel = new GameboardViewModel(game, locationProvider, worldMapViewModelFactory, territoryViewModelUpdater);

            //var worldMapViewModel = new WorldMapViewModel();
            //worldMapViewModels.Apply(worldMapViewModel.WorldMapViewModels.Add);

            return gameboardViewModel;
        }
    }
}