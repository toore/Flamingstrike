using System.Linq;
using GuiWpf.Services;
using GuiWpf.Territories;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.Extensions;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;

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
            var territories = new RISK.Application.Territories();
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(territories, colorService);
            var territoryLayoutInformationFactory = new TerritoryGuiFactory(territories);
            var territoryViewModelUpdater = new TerritoryViewModelUpdater(territoryColorsFactory, colorService);
            var territoryViewModelFactory = new TerritoryViewModelFactory(territoryViewModelUpdater, territoryLayoutInformationFactory);

            var territory = territories.Brazil;
            var humanPlayer = new HumanPlayer("pelle");
            territory.Occupant = humanPlayer;
            territory.Armies = 99;

            var textViewModelFactory = new TerritoryTextViewModelFactory(territoryLayoutInformationFactory);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(territoryViewModelFactory, textViewModelFactory);

            var playerProvider = new Players();
            playerProvider.SetPlayers(humanPlayer.AsList());

            IInteractionStateFactory interactionStateFactory = new InteractionStateFactory(null);
            var game = new Game(interactionStateFactory, new StateControllerFactory(),  playerProvider, territories, new CardFactory());
            var gameboardViewModel = new GameboardViewModel(game, territories.GetAll(), worldMapViewModelFactory, territoryViewModelUpdater, null, new GameOverViewModelFactory(), null, null);

            return gameboardViewModel;
        }

        public ITerritory SelectLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            return locationSelectorParameter.EnabledTerritories.First();
        }
    }
}