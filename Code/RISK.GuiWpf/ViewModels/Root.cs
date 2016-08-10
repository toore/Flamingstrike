using Caliburn.Micro;
using GuiWpf.RegionModels;
using GuiWpf.Services;
using GuiWpf.ViewModels.AlternateSetup;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Preparation;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.GameEngine.Setup;
using Toore.Shuffling;

namespace GuiWpf.ViewModels
{
    public class Root
    {
        public PlayerRepository PlayerRepository { get; }
        public IEventAggregator EventAggregator { get; }
        public IAlternateGameSetupFactory AlternateGameSetupFactory { get; private set; }
        public IGamePreparationViewModelFactory GamePreparationViewModelFactory { get; private set; }
        public IGameboardViewModelFactory GameboardViewModelFactory { get; private set; }
        public IAlternateGameSetupViewModelFactory AlternateGameSetupViewModelFactory { get; private set; }
        public IUserInteractorFactory UserInteractorFactory { get; set; }

        public Root() : this(
            taskEx: new TaskEx()) {}

        public Root(ITaskEx taskEx)
        {
            var playerFactory = new PlayerFactory();
            var playerTypes = new PlayerTypes();
            PlayerRepository = new PlayerRepository();
            EventAggregator = new EventAggregator();

            GamePreparationViewModelFactory = new GamePreparationViewModelFactory(
                playerFactory,
                playerTypes,
                PlayerRepository,
                EventAggregator);

            var interactionStateFsm = new InteractionStateFsm();
            var interactionStateFactory = new InteractionStateFactory(interactionStateFsm);
            var colorService = new ColorService();
            var continents = new Continents();
            var regions = new Regions(continents);
            var regionColorSettingsFactory = new RegionColorSettingsFactory(colorService, regions);
            var regionModelFactory = new RegionModelFactory(regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(
                regionModelFactory, regionColorSettingsFactory, colorService);
            var windowManager = new WindowManager();
            var gameOverViewModelFactory = new GameOverViewModelFactory();
            var screenConfirmationService = new ScreenConfirmationService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenConfirmationService);
            var userNotifier = new UserNotifier(windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);

            GameboardViewModelFactory = new GameboardViewModelFactory(
                interactionStateFsm,
                interactionStateFactory,
                regions,
                worldMapViewModelFactory,
                windowManager,
                gameOverViewModelFactory,
                dialogManager,
                EventAggregator);

            var randomWrapper = new RandomWrapper();
            var shuffle = new FisherYatesShuffle(randomWrapper);
            var deckFactory = new DeckFactory(regions, shuffle);
            var battleCalculator = new ArmiesLostCalculator();
            var dice = new Dice(randomWrapper);
            var diceRoller = new DicesRoller(dice);
            var battle = new Battle(diceRoller, battleCalculator);
            var gameDataFactory = new GameDataFactory();
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var gameRules = new GameRules();
            var gameStateFactory = new GameStateFactory(gameDataFactory, armyDrafter, territoryOccupier, attacker, fortifier, gameRules);
            var armyDraftCalculator = new ArmyDraftCalculator(continents);
            var gameStateFsm = new GameStateFsm();
            var gameStateConductor = new GameStateConductor(gameStateFactory, armyDraftCalculator, gameDataFactory, gameStateFsm);
            var gameFactory = new GameFactory(gameDataFactory, gameStateConductor, deckFactory, gameStateFsm, gameRules);

            AlternateGameSetupViewModelFactory = new AlternateGameSetupViewModelFactory(
                gameFactory,
                worldMapViewModelFactory,
                dialogManager,
                EventAggregator,
                taskEx);

            var startingInfantryCalculator = new StartingInfantryCalculator();

            AlternateGameSetupFactory = new AlternateGameSetupFactory(
                regions, shuffle, startingInfantryCalculator);

            var userInteractionFactory = new UserInteractionFactory();
            var guiThreadDispatcher = new GuiThreadDispatcher();
            UserInteractorFactory = new UserInteractorFactory(userInteractionFactory, guiThreadDispatcher);
        }
    }
}