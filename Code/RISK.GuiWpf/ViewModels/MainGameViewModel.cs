using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<SetupGameMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameInitializationViewModelFactory _gameInitializationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly AlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly PlayerRepository _playerRepository;

        public MainGameViewModel()
            : this(new Root()) {}

        protected MainGameViewModel(Root root)
            : this(
                new GameInitializationViewModelFactory(
                    root.PlayerIdFactory,
                    root.PlayerTypes,
                    root.PlayerRepository,
                    root.EventAggregator),
                new GameboardViewModelFactory(
                    root.WorldMap,
                    root.WorldMapViewModelFactory,
                    root.WindowManager,
                    root.GameOverViewModelFactory,
                    root.DialogManager,
                    root.EventAggregator),
                new GameSetupViewModelFactory(
                    root.GameFactory,
                    root.GameAdapterFactory,
                    root.WorldMapViewModelFactory,
                    root.DialogManager,
                    root.EventAggregator,
                    root.UserInteractor,
                    root.GuiThreadDispatcher,
                    root.TaskEx),
                root.AlternateGameSetupFactory,
                root.PlayerRepository)
        {
            root.EventAggregator.Subscribe(this);
        }

        private MainGameViewModel(IGameInitializationViewModelFactory gameInitializationViewModelFactory, IGameboardViewModelFactory gameboardViewModelFactory, IGameSetupViewModelFactory gameSetupViewModelFactory, AlternateGameSetupFactory alternateGameSetupFactory, PlayerRepository playerRepository)
        {
            _gameInitializationViewModelFactory = gameInitializationViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;
            _alternateGameSetupFactory = alternateGameSetupFactory;
            _playerRepository = playerRepository;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeNewGame();
        }

        public override string DisplayName
        {
            get { return "R!SK"; }
            set { }
        }

        public void Handle(NewGameMessage newGameMessage)
        {
            InitializeNewGame();
        }

        public void Handle(SetupGameMessage setupGameMessage)
        {
            StartGameSetup();
        }

        public void Handle(StartGameplayMessage startGameplayMessage)
        {
            StartGamePlay(startGameplayMessage.GameAdapter);
        }

        private void InitializeNewGame()
        {
            var gameSettingsViewModel = _gameInitializationViewModelFactory.Create();
            ActivateItem(gameSettingsViewModel);
        }

        private void StartGameSetup()
        {
            var players = _playerRepository.GetAll();
            var alternateGameSetup = _alternateGameSetupFactory.Create(players);
            var gameSetupViewModel = _gameSetupViewModelFactory.Create(alternateGameSetup);

            ActivateItem(gameSetupViewModel);
        }

        private void StartGamePlay(IGameAdapter gameAdapter)
        {
            var gameboardViewModel = _gameboardViewModelFactory.Create(gameAdapter);
            ActivateItem(gameboardViewModel);
        }
    }
}