using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
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
        public IGamePreparationViewModelFactory GamePreparationViewModelFactory { get; }
        public IGameplayViewModelFactory GameplayViewModelFactory { get; }
        public IAlternateGameSetupViewModelFactory AlternateGameSetupViewModelFactory { get; }
        public IGameEngineClientProxy GameEngineClientProxy { get; }

        public CompositionRoot()
        {
            var playerTypes = new PlayerTypes();
            PlayerUiDataRepository = new PlayerUiDataRepository();
            EventAggregator = new EventAggregator();

            GamePreparationViewModelFactory = new GamePreparationViewModelFactory(
                playerTypes,
                PlayerUiDataRepository,
                EventAggregator);

            var regionModelFactory = new RegionModelFactory();
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

            //GameEngineClientProxy = CreateInternalGameEngine();
            GameEngineClientProxy = new GameEngineProxy();

            AlternateGameSetupViewModelFactory = new AlternateGameSetupViewModelFactory(
                worldMapViewModelFactory,
                PlayerUiDataRepository,
                dialogManager,
                EventAggregator,
                GameEngineClientProxy);
        }

        private GameEngineAdapter CreateInProcessGameEngine()
        {
            var randomWrapper = new RandomWrapper();
            var shuffler = new FisherYatesShuffler(randomWrapper);
            var worldMapFactory = new WorldMapFactory();
            var worldMap = worldMapFactory.Create();
            var deckFactory = new DeckFactory(worldMap.GetAll(), shuffler);
            var armiesLostCalculator = new ArmiesLostCalculator();
            var die = new Die(randomWrapper);
            var dice = new Dice(die);
            var attackService = new AttackService(worldMap, dice, armiesLostCalculator);
            var playerEliminationRules = new PlayerEliminationRules();
            var armyDraftCalculator = new ArmyDraftCalculator(new[] { Continent.Asia, Continent.NorthAmerica, Continent.Europe, Continent.Africa, Continent.Australia, Continent.SouthAmerica });
            var gamePhaseFactory = new GamePhaseFactory(attackService, playerEliminationRules, worldMap);

#if QUICK_SETUP
            var startingInfantryCalculator = new StartingInfantryCalculatorReturning22Armies();
#else
            var startingInfantryCalculator = new StartingInfantryCalculator();
#endif

            var alternateGameSetupBootstrapper = new AlternateGameSetupBootstrapper(worldMap.GetAll(), shuffler, startingInfantryCalculator);
            var gameBootstrapper = new GameBootstrapper(gamePhaseFactory, armyDraftCalculator, deckFactory);

            return new GameEngineAdapter(alternateGameSetupBootstrapper, gameBootstrapper);
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