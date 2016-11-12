using Caliburn.Micro;
using RISK.GameEngine;
using RISK.GameEngine.Attacking;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using RISK.GameEngine.Setup;
using RISK.GameEngine.Shuffling;
using RISK.UI.WPF.RegionModels;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.AlternateSetup;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.ViewModels
{
    public class Root
    {
        public PlayerUiDataRepository PlayerUiDataRepository { get; }
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
            PlayerUiDataRepository = new PlayerUiDataRepository();
            EventAggregator = new EventAggregator();

            GamePreparationViewModelFactory = new GamePreparationViewModelFactory(
                playerFactory,
                playerTypes,
                PlayerUiDataRepository,
                EventAggregator);

            var continents = new Continents();
            var regions = new Regions(continents);
            var regionModelFactory = new RegionModelFactory(regions);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, PlayerUiDataRepository);
            var windowManager = new WindowManager();
            var screenConfirmationService = new ScreenConfirmationService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenConfirmationService);
            var userNotifier = new UserNotifier(windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);
            var interactionStateFactory = new InteractionStateFactory();

            GameplayViewModelFactory = new GameplayViewModelFactory(
                interactionStateFactory,
                worldMapViewModelFactory,
                PlayerUiDataRepository,
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
            var gameRules = new PlayerEliminationRules();
            var gameStateFactory = new GameStateFactory(gameRules, armyDrafter, attacker, territoryOccupier, fortifier);
            var armyDraftCalculator = new ArmyDraftCalculator(continents);

            AlternateGameSetupViewModelFactory = new AlternateGameSetupViewModelFactory(
                worldMapViewModelFactory,
                PlayerUiDataRepository,
                dialogManager,
                EventAggregator);

#if QUICKSETUP
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