using System.Linq;
using GuiWpf.RegionModels;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.Views.WorldMapViews
{
    public class GameboardViewModelTestDataFactory : ITerritoryResponder
    {
        public static GameboardViewModel ViewModel => new GameboardViewModelTestDataFactory().Create();

        private GameboardViewModel Create()
        {
            var continents = new Continents();
            var regions = new Regions(continents);
            var regionModelFactory = new RegionModelFactory(regions);
            var colorService = new ColorService();
            var regionColorSettingFactory = new RegionColorSettingsFactory(colorService, regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, regionColorSettingFactory, colorService);
            var game = new Game(null, null);
            var interactionStateFsm = new InteractionStateFsm();
            var interactionStateFactory = new InteractionStateFactory(interactionStateFsm);
            var gameboardViewModel = new GameboardViewModel(game, interactionStateFsm, interactionStateFactory, regions, worldMapViewModelFactory, null, null, null, null);
            gameboardViewModel.Activate();

            return gameboardViewModel;
        }

        public IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            return territoryRequestParameter.EnabledTerritories.First();
        }
    }
}