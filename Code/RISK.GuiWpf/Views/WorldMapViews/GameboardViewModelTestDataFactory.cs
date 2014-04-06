using System.Linq;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.Views.WorldMapViews
{
    public class GameboardViewModelTestDataFactory : IGameInitializerLocationSelector
    {
        public static GameboardViewModel ViewModel
        {
            get { return new GameboardViewModelTestDataFactory().Create(); }
        }

        private GameboardViewModel Create()
        {
            var continents = new Continents();
            var locations = new Locations(continents);
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(locations, colorService);
            var territoryLayoutInformationFactory = new TerritoryGuiFactory(locations);
            var territoryViewModelUpdater = new TerritoryViewModelUpdater(territoryColorsFactory, colorService);
            var territoryViewModelFactory = new TerritoryViewModelFactory(territoryViewModelUpdater, territoryLayoutInformationFactory);

            var worldMap = new WorldMap(locations);
            var territory = worldMap.GetTerritory(locations.Brazil);
            var humanPlayer = new HumanPlayer("pelle");
            territory.Occupant = humanPlayer;
            territory.Armies = 99;

            var textViewModelFactory = new TerritoryTextViewModelFactory(territoryLayoutInformationFactory);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(locations, territoryViewModelFactory, textViewModelFactory);

            var playerProvider = new Players();
            playerProvider.SetPlayers(humanPlayer.AsList());

            var alternateGameSetup = new AlternateGameSetup(playerProvider, locations, new RandomSorter(new RandomWrapper()), new WorldMapFactory(locations), new InitialArmyCount());
            ITurnFactory turnFactory = new TurnFactory(null, null);
            var game = new Game(turnFactory, playerProvider, alternateGameSetup, this);
            var gameboardViewModel = new GameboardViewModel(game, locations.GetAll(), worldMapViewModelFactory, territoryViewModelUpdater, new FakeGameOverEvaluater(), null, new GameOverViewModelFactory(), new ResourceManagerWrapper(), null, null);

            return gameboardViewModel;
        }

        public ILocation SelectLocation(ILocationSelectorParameter locationSelectorParameter)
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