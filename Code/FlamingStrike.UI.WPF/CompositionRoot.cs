using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Play.GameStates;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using Toore.Shuffling;

namespace FlamingStrike.UI.WPF
{
    public class CompositionRoot
    {
        public PlayerUiDataRepository PlayerUiDataRepository { get; }
        public IEventAggregator EventAggregator { get; }
        public IAlternateGameSetupFactory AlternateGameSetupFactory { get; }
        public IGamePreparationViewModelFactory GamePreparationViewModelFactory { get; }
        public IGameplayViewModelFactory GameplayViewModelFactory { get; private set; }
        public IAlternateGameSetupViewModelFactory AlternateGameSetupViewModelFactory { get; }
        public IGameFactory GameFactory { get; }

        public CompositionRoot()
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
            var playerStatusViewModelFactory = new PlayerStatusViewModelFactory(PlayerUiDataRepository);

            GameplayViewModelFactory = new GameplayViewModelFactory(
                interactionStateFactory,
                worldMapViewModelFactory,
                PlayerUiDataRepository,
                dialogManager,
                EventAggregator,
                playerStatusViewModelFactory);

            var randomWrapper = new RandomWrapper();
            var shuffle = new FisherYatesShuffler(randomWrapper);
            var deckFactory = new DeckFactory(regions, shuffle);
            var battleCalculator = new ArmiesLostCalculator();
            var dice = new Die(randomWrapper);
            var diceRoller = new Dice(dice);
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
            var startingInfantryCalculator = new StartingInfantryCalculatorReturning22Armies();
#else
            var startingInfantryCalculator = new StartingInfantryCalculator();
#endif

            AlternateGameSetupFactory = new AlternateGameSetupFactory(regions, shuffle, startingInfantryCalculator);

            GameFactory = new GameFactory(gameStateFactory, armyDraftCalculator, deckFactory, armyDrafter);
        }
    }

    public class StartingInfantryCalculatorReturning22Armies : IStartingInfantryCalculator
    {
        public int Get(int numberOfPlayers)
        {
            return 22;
        }
    }
}