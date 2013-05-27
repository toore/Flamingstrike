using System.Linq;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Base.Extensions;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.Views.WorldMapViews
{
    public class GameboardViewModelTestDataFactory : ILocationSelector
    {
        public static GameboardViewModel ViewModel
        {
            get { return new GameboardViewModelTestDataFactory().Create(); }
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
            territory.Occupant = humanPlayer;
            territory.Armies = 99;

            var textViewModelFactory = new TerritoryTextViewModelFactory(territoryLayoutInformationFactory);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(locationProvider, territoryViewModelFactory, textViewModelFactory);

            var playerProvider = new PlayerProvider();
            playerProvider.All = humanPlayer.AsList();

            var alternateGameSetup = new AlternateGameSetup(playerProvider, locationProvider, new RandomSorter(new RandomWrapper()), new WorldMapFactory(locationProvider), new InitialArmyCountProvider());
            ITurnFactory turnFactory = new TurnFactory(null, null);
            var game = new Game(turnFactory, playerProvider, alternateGameSetup, this);
            var gameboardViewModel = new GameboardViewModel(game, locationProvider, worldMapViewModelFactory, territoryViewModelUpdater, new FakeGameOverEvaluater(), null, new GameOverViewModelFactory(), new ResourceManagerWrapper(), null, null);

            return gameboardViewModel;
        }

        public ILocation GetLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            return locationSelectorParameter.AvailableLocations.First();
        }

        private class FakeGameOverEvaluater : IGameOverEvaluater
        {
            public bool IsGameOver(IWorldMap worldMap)
            {
                return false;
            }
        }
    }
}