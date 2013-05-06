using System.Linq;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModelTestData : ILocationSelector
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
            var territoryLayoutInformationFactory = new TerritoryGuiFactory(locationProvider);
            var territoryViewModelUpdater = new TerritoryViewModelUpdater(territoryColorsFactory, colorService);
            var territoryViewModelFactory = new TerritoryViewModelFactory(territoryViewModelUpdater, territoryLayoutInformationFactory);

            var worldMap = new WorldMap(locationProvider);
            var territory = worldMap.GetTerritory(locationProvider.Brazil);
            var humanPlayer = new HumanPlayer("pelle");
            territory.AssignedPlayer = humanPlayer;
            territory.Armies = 99;

            var textViewModelFactory = new TerritoryTextViewModelFactory(territoryLayoutInformationFactory);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(locationProvider, territoryViewModelFactory, textViewModelFactory);

            var playerRepository = new PlayerRepository();
            playerRepository.Add(humanPlayer);

            var alternateGameSetup = new AlternateGameSetup(playerRepository, locationProvider, new RandomSorter(new RandomWrapper()), new WorldMapFactory(locationProvider), new InitialArmyCountProvider());
            ITurnFactory turnFactory = new TurnFactory(null, null);
            var game = new Game(turnFactory, playerRepository, alternateGameSetup, this);
            var gameboardViewModel = new GameboardViewModel(game, locationProvider, worldMapViewModelFactory, territoryViewModelUpdater, new GameOverEvaluater(), null, null);

            return gameboardViewModel;
        }

        public ILocation GetLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            return locationSelectorParameter.AvailableLocations.First();
        }
    }
}