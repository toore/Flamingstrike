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
using RISK.GameEngine.Play.GameStates;
using RISK.GameEngine.Setup;
using Toore.Shuffling;

namespace GuiWpf.ViewModels
{
    public class Root
    {
        public PlayerRepository PlayerRepository { get; }
        public IEventAggregator EventAggregator { get; }
        public IAlternateGameSetupFactory AlternateGameSetupFactory { get; }
        public IGamePreparationViewModelFactory GamePreparationViewModelFactory { get; }
        public IGameplayViewModelFactory GameplayViewModelFactory { get; private set; }
        public IAlternateGameSetupViewModelFactory AlternateGameSetupViewModelFactory { get; }
        public IGameFactory GameFactory { get; }

        public Root()
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

            var colorService = new ColorService();
            var continents = new Continents();
            var regions = new Regions(continents);
            var regionColorSettingsFactory = new RegionColorSettingsFactory(colorService, regions);
            var regionModelFactory = new RegionModelFactory(regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, regionColorSettingsFactory, colorService);
            var windowManager = new WindowManager();
            var gameOverViewModelFactory = new GameOverViewModelFactory();
            var screenConfirmationService = new ScreenConfirmationService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenConfirmationService);
            var userNotifier = new UserNotifier(windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);
            var interactionStateFactory = new InteractionStateFactory();

            GameplayViewModelFactory = new GameplayViewModelFactory(
                interactionStateFactory,
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
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var gameRules = new GameRules();
            var gameStateFactory = new GameStateFactory(gameRules, armyDrafter, attacker, territoryOccupier, fortifier);
            var armyDraftCalculator = new ArmyDraftCalculator(continents);

            AlternateGameSetupViewModelFactory = new AlternateGameSetupViewModelFactory(
                worldMapViewModelFactory,
                dialogManager,
                EventAggregator);

#if QUICK_SETUP
            var startingInfantryCalculator = new StartingInfantryCalculatorReturning21Armies();
#else
            var startingInfantryCalculator = new StartingInfantryCalculator();
#endif

            AlternateGameSetupFactory = new AlternateGameSetupFactory(regions, shuffle, startingInfantryCalculator);

            GameFactory = new GameFactory(gameStateFactory, armyDraftCalculator, deckFactory);
        }
    }

    public class StartingInfantryCalculatorReturning21Armies : IStartingInfantryCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return 22;
        }
    }
}