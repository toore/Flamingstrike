using System.Linq;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.Views.WorldMapViews
{
    public class GameboardViewModelTestDataFactory : ITerritoryRequestHandler
    {
        public static GameboardViewModel ViewModel => new GameboardViewModelTestDataFactory().Create();

        private GameboardViewModel Create()
        {
            //var worldMap = new WorldMap();
            //var colorService = new ColorService();
            //var territoryColorsFactory = new TerritoryColorsFactory(colorService);

            //var brazil = worldMap.Brazil;
            //var humanPlayer = new Human("pelle");
            //brazil.Occupant = humanPlayer;
            //brazil.Armies = 99;
            //var humanPlayer2 = new Human("kalle");
            //var alaska = worldMap.Alaska;
            //alaska.Occupant = humanPlayer2;
            //alaska.Armies = 11;

            //var worldMapViewModelFactory = new WorldMapViewModelFactory(new WorldMapModelFactory(), territoryColorsFactory, colorService);

            //var players = new IPlayerId[] { humanPlayer, humanPlayer2 }.OrderBy(x => x.PlayerOrderIndex);
            //var game = new Game(players, worldMap, new CardFactory(), null);
            //var interactionStateFactory = new InteractionStateFactory();
            //var gameAdapter = new GameAdapter(interactionStateFactory, new StateControllerFactory(interactionStateFactory), game);
            //var gameboardViewModel = new GameboardViewModel(gameAdapter, worldMapViewModelFactory, null, new GameOverViewModelFactory(), null, null);

            //return gameboardViewModel;

            return null;
        }

        public ITerritory ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            return territoryRequestParameter.EnabledTerritories.First();
        }
    }
}