using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Setup;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;

namespace GuiWpf.ViewModels
{
    public class Root
    {
        public PlayerRepository PlayerRepository { get; }
        public IEventAggregator EventAggregator { get; }
        public IAlternateGameSetupFactory AlternateGameSetupFactory { get; private set; }
        public IGameInitializationViewModelFactory GameInitializationViewModelFactory { get; private set; }
        public IGameboardViewModelFactory GameboardViewModelFactory { get; private set; }
        public IGameSetupViewModelFactory GameSetupViewModelFactory { get; private set; }
        public IUserInteractorFactory UserInteractorFactory { get; set; }

        public Root() : this(
            taskEx: new TaskEx()) {}

        public Root(ITaskEx taskEx)
        {
            var playerIdFactory = new PlayerIdFactory();
            var playerTypes = new PlayerTypes();
            PlayerRepository = new PlayerRepository();
            EventAggregator = new EventAggregator();

            GameInitializationViewModelFactory = new GameInitializationViewModelFactory(
                playerIdFactory,
                playerTypes,
                PlayerRepository,
                EventAggregator);

            var stateControllerFactory = new StateControllerFactory();
            var interactionStateFactory = new InteractionStateFactory();
            var colorService = new ColorService();
            var worldMap = new WorldMap();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService, worldMap);
            var worldMapModelFactory = new WorldMapModelFactory();
            var worldMapViewModelFactory = new WorldMapViewModelFactory(
                worldMap, worldMapModelFactory, territoryColorsFactory, colorService);
            var windowManager = new WindowManager();
            var gameOverViewModelFactory = new GameOverViewModelFactory();
            var screenService = new ScreenService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);

            GameboardViewModelFactory = new GameboardViewModelFactory(
                stateControllerFactory,
                interactionStateFactory,
                worldMap,
                worldMapViewModelFactory,
                windowManager,
                gameOverViewModelFactory,
                dialogManager,
                EventAggregator);

            var gameboardRules = new GameRules();
            var cardFactory = new CardFactory();
            var battleCalculator = new BattleOutcomeCalculator();
            var randomWrapper = new RandomWrapper();
            var dice = new Dice(randomWrapper);
            var diceRoller = new DicesRoller(dice);
            var battle = new Battle(diceRoller, battleCalculator);
            var gameFactory = new GameFactory(gameboardRules, cardFactory, battle, new PlayerFactory());

            GameSetupViewModelFactory = new GameSetupViewModelFactory(
                gameFactory,
                worldMapViewModelFactory,
                dialogManager,
                EventAggregator,
                taskEx);

            var shuffler = new FisherYatesShuffler(randomWrapper);
            var startingInfantryCalculator = new StartingInfantryCalculator();

            AlternateGameSetupFactory = new AlternateGameSetupFactory(
                worldMap, shuffler, startingInfantryCalculator);

            var userInteractionFactory = new UserInteractionFactory();
            var guiThreadDispatcher = new GuiThreadDispatcher();
            UserInteractorFactory = new UserInteractorFactory(userInteractionFactory, guiThreadDispatcher);
        }
    }
}