using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.GameEngine.Setup;
using IPlayer = RISK.GameEngine.IPlayer;

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
            var territoryColorsFactory = new TerritoryColorsFactory(colorService, regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, territoryColorsFactory, colorService);
            var players = new Sequence<IPlayer>(new Player("player 1"), new Player("player 1"));
            IReadOnlyList<ITerritory> initialTerritories = regions.GetAll().Select(region => new Territory(region, players.Next(), new Random().Next(99))).ToList();
            var gameStateConductor = new GameStateConductor(null, null, null, null);
            var game = new Game(null, gameStateConductor, null, players, initialTerritories, new Deck(new[] { new WildCard() }));
            var stateControllerFactory = new StateControllerFactory();
            var interactionStateFactory = new InteractionStateFactory();
            var gameboardViewModel = new GameboardViewModel(game, stateControllerFactory, interactionStateFactory, regions, worldMapViewModelFactory, null, null, null, null);
            gameboardViewModel.Activate();

            return gameboardViewModel;
        }

        public IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            return territoryRequestParameter.EnabledTerritories.First();
        }
    }
}