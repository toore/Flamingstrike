using System.Linq;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.Views.WorldMapViews
{
    public class GameboardViewModelTestDataFactory : ITerritorySelector
    {
        public static GameboardViewModel ViewModel
        {
            get { return new GameboardViewModelTestDataFactory().Create(); }
        }

        private GameboardViewModel Create()
        {
            var worldMap = new WorldMap();
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService);
            var territoryViewModelUpdater = new TerritoryViewModelColorInitializer(territoryColorsFactory, colorService);

            var brazil = worldMap.Brazil;
            var humanPlayer = new HumanPlayer("pelle");
            brazil.Occupant = humanPlayer;
            brazil.Armies = 99;
            var humanPlayer2 = new HumanPlayer("kalle");
            var alaska = worldMap.Alaska;
            alaska.Occupant = humanPlayer2;
            alaska.Armies = 11;

            var worldMapViewModelFactory = new WorldMapViewModelFactory(new WorldMapModelFactory(), new TerritoryViewModelColorInitializer(territoryColorsFactory, colorService));

            var interactionStateFactory = new InteractionStateFactory(null);
            var game = new Game(interactionStateFactory, new StateControllerFactory(interactionStateFactory), new[] { humanPlayer, humanPlayer2 }, worldMap, new CardFactory());
            var gameboardViewModel = new GameboardViewModel(game, worldMapViewModelFactory, territoryViewModelUpdater, null, new GameOverViewModelFactory(), null, null);

            return gameboardViewModel;
        }

        public ITerritory SelectTerritory(ITerritorySelectorParameter territorySelectorParameter)
        {
            return territorySelectorParameter.EnabledTerritories.First();
        }
    }
}