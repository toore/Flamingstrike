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
            var locations = new Locations();
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

            IInteractionStateFactory interactionStateFactory = new InteractionStateFactory(null);
            var game = new Game(interactionStateFactory, new StateControllerFactory(),  playerProvider, worldMap, new CardFactory());
            var gameboardViewModel = new GameboardViewModel(game, locations.GetAll(), worldMapViewModelFactory, territoryViewModelUpdater, null, new GameOverViewModelFactory(), null, null);

            return gameboardViewModel;
        }

        public ILocation SelectLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            return locationSelectorParameter.AvailableLocations.First();
        }
    }
}