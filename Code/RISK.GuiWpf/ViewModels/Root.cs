using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
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
        public IUserInteractor UserInteractor { get; set; }
        public GameFactoryWorker GameFactoryWorker { get; private set; }
        public DialogManager DialogManager { get; private set; }
        public GameOverViewModelFactory GameOverViewModelFactory { get; private set; }
        public WindowManager WindowManager { get; private set; }
        public TerritoryViewModelColorInitializer TerritoryViewModelColorInitializer { get; private set; }
        public WorldMapViewModelFactory WorldMapViewModelFactory { get; private set; }
        public PlayerFactory PlayerFactory { get; private set; }
        public PlayerTypes PlayerTypes { get; private set; }
        public PlayerRepository PlayerRepository { get; private set; }
        public EventAggregator EventAggregator { get; private set; }

        public Root()
        {
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService);
            var worldMapModelFactory = new WorldMapModelFactory();
            TerritoryViewModelColorInitializer = new TerritoryViewModelColorInitializer(territoryColorsFactory, colorService);
            WorldMapViewModelFactory = new WorldMapViewModelFactory(worldMapModelFactory, TerritoryViewModelColorInitializer);
            GameOverViewModelFactory = new GameOverViewModelFactory();

            var screenService = new ScreenService();
            var randomWrapper = new RandomWrapper();
            var randomSorter = new RandomSorter(randomWrapper);
            var casualtiesCalculator = new CasualtiesCalculator();
            var dice = new Dice(randomWrapper);
            var dices = new Dices(casualtiesCalculator, dice);
            var cardFactory = new CardFactory();
            var battleCalculator = new BattleCalculator(dices);
            var interactionStateFactory = new InteractionStateFactory(battleCalculator);
            var stateControllerFactory = new StateControllerFactory(interactionStateFactory);
            var initialArmyAssignmentCalculator = new InitialArmyAssignmentCalculator();
            var worldMapFactory = new WorldMapFactory();
            PlayerRepository = new PlayerRepository();
            var alternateGameSetup = new AlternateGameSetup(PlayerRepository, worldMapFactory, randomSorter, initialArmyAssignmentCalculator);

            WindowManager = new WindowManager();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(WindowManager, confirmViewModelFactory);
            DialogManager = new DialogManager(userNotifier);

            var gameFactory = new GameFactory(alternateGameSetup, interactionStateFactory, stateControllerFactory, PlayerRepository, cardFactory);
            GameFactoryWorker = new GameFactoryWorker(gameFactory);

            PlayerFactory = new PlayerFactory();
            PlayerTypes = new PlayerTypes();
            EventAggregator = new EventAggregator();

            UserInteractor = new UserInteractor();
        }
    }
}