using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GamePlay;
using RISK.Application.GamePlay.Battling;
using RISK.Application.GameSetup;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;

namespace GuiWpf.ViewModels
{
    public class Root
    {
        public IUserInteractor UserInteractor { get; set; }
        public DialogManager DialogManager { get; private set; }
        public GameOverViewModelFactory GameOverViewModelFactory { get; private set; }
        public WindowManager WindowManager { get; }
        public WorldMapViewModelFactory WorldMapViewModelFactory { get; private set; }
        public PlayerIdFactory PlayerIdFactory { get; private set; }
        public PlayerTypes PlayerTypes { get; private set; }
        public PlayerRepository PlayerRepository { get; }
        public IEventAggregator EventAggregator { get; private set; }
        public IGuiThreadDispatcher GuiThreadDispatcher { get; set; }
        public ITaskEx TaskEx { get; set; }
        public AlternateGameSetupFactory AlternateGameSetupFactory { get; set; }

        public Root()
        {
            var colorService = new ColorService();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService);
            var worldMapModelFactory = new WorldMapModelFactory();
            WorldMapViewModelFactory = new WorldMapViewModelFactory(worldMapModelFactory, territoryColorsFactory, colorService);
            GameOverViewModelFactory = new GameOverViewModelFactory();

            var screenService = new ScreenService();
            var randomWrapper = new RandomWrapper();
            var shuffler = new FisherYatesShuffler(randomWrapper);
            var dice = new Dice(randomWrapper);
            var diceRoller = new DiceRoller(dice);
            var cardFactory = new CardFactory();
            var battle = new Battle(diceRoller, new BattleCalculator());
            var interactionStateFactory = new InteractionStateFactory();
            var stateControllerFactory = new StateControllerFactory(interactionStateFactory);
            var startingInfantryCalculator = new StartingInfantryCalculator();

            PlayerRepository = new PlayerRepository();

            var worldMap = new WorldMap();
            AlternateGameSetupFactory = new AlternateGameSetupFactory(worldMap, shuffler, startingInfantryCalculator);

            WindowManager = new WindowManager();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(WindowManager, confirmViewModelFactory);
            DialogManager = new DialogManager(userNotifier);

            PlayerIdFactory = new PlayerIdFactory();
            PlayerTypes = new PlayerTypes();
            EventAggregator = new EventAggregator();

            UserInteractor = new UserInteractor();

            GuiThreadDispatcher = new GuiThreadDispatcher();
            TaskEx = new TaskEx();
        }
    }
}