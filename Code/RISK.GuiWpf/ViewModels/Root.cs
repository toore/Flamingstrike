using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.DiceAndCalculation;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels
{
    public class Root
    {
        public EventAggregator EventAggregator { get; private set; }

        public Root()
        {
            var playerFactory = new PlayerFactory();
            var playerTypes = new PlayerTypes();
            EventAggregator = new EventAggregator();
            var playerRepository = new PlayerRepository();

            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService);
            var worldMapModelFactory = new WorldMapModelFactory();
            var territoryViewModelColorInitializer = new TerritoryViewModelColorInitializer(territoryColorsFactory, colorService);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(worldMapModelFactory, territoryViewModelColorInitializer);
            var windowManager = new WindowManager();
            var gameOverViewModelFactory = new GameOverViewModelFactory();

            var screenService = new ScreenService();
            var randomWrapper = new RandomWrapper();
            var randomSorter = new RandomSorter(randomWrapper);
            var userInteractor = new UserInteractor();
            var casualtiesCalculator = new CasualtiesCalculator();
            var dice = new Dice(randomWrapper);
            var dices = new Dices(casualtiesCalculator, dice);
            var cardFactory = new CardFactory();
            var stateControllerFactory = new StateControllerFactory();
            var battleCalculator = new BattleCalculator(dices);
            var interactionStateFactory = new InteractionStateFactory(battleCalculator);
            var initialArmyAssignmentCalculator = new InitialArmyAssignmentCalculator();
            var worldMapFactory = new WorldMapFactory();
            var alternateGameSetup = new AlternateGameSetup(playerRepository, worldMapFactory, randomSorter, initialArmyAssignmentCalculator);

            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);

            var gameFactory = new GameFactory(alternateGameSetup, interactionStateFactory, stateControllerFactory, playerRepository, cardFactory);
            var gameFactoryWorker = new GameFactoryWorker(gameFactory);

            GameSettingsViewModelFactory = new GameSettingsViewModelFactory(playerFactory, playerTypes, playerRepository, EventAggregator);
            GameboardViewModelFactory = new GameboardViewModelFactory(worldMapViewModelFactory, territoryViewModelColorInitializer, windowManager, gameOverViewModelFactory, dialogManager, EventAggregator);
            GameSetupViewModelFactory = new GameSetupViewModelFactory(worldMapViewModelFactory, dialogManager, EventAggregator, userInteractor, gameFactoryWorker);

        }

        public GameSettingsViewModelFactory GameSettingsViewModelFactory { get; private set; }
        public GameboardViewModelFactory GameboardViewModelFactory { get; private set; }
        public GameSetupViewModelFactory GameSetupViewModelFactory { get; private set; }
    }
}