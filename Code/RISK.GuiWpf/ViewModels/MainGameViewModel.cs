using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<SetupGameMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameInitializationViewModelFactory _gameInitializationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel()
            : this(new Root()) { }

        protected MainGameViewModel(Root root)
            : this(
                new GameInitializationViewModelFactory(
                root.PlayerFactory,
                root.PlayerTypes,
                root.PlayerRepository,
                root.EventAggregator),

                new GameboardViewModelFactory(
                root.WorldMapViewModelFactory,
                root.TerritoryViewModelColorInitializer,
                root.WindowManager,
                root.GameOverViewModelFactory,
                root.DialogManager,
                root.EventAggregator),

                new GameSetupViewModelFactory(
                root.WorldMapViewModelFactory,
                root.DialogManager,
                root.EventAggregator,
                root.UserInteractor,
                root.GameFactoryWorker))
        {
            root.EventAggregator.Subscribe(this);
        }

        public MainGameViewModel(
            IGameInitializationViewModelFactory gameInitializationViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IGameSetupViewModelFactory gameSetupViewModelFactory)
        {
            _gameInitializationViewModelFactory = gameInitializationViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeNewGame();
        }

        public override string DisplayName
        {
            get { return "Yarc"; }
            set { }
        }

        public void Handle(NewGameMessage newGameMessage)
        {
            InitializeNewGame();
        }

        public void Handle(SetupGameMessage setupGameMessage)
        {
            StartGame();
        }

        public void Handle(StartGameplayMessage startGameplayMessage)
        {
            StartGamePlay(startGameplayMessage.Game);
        }

        private void InitializeNewGame()
        {
            ActivateItem(_gameInitializationViewModelFactory.Create());
        }

        private void StartGame()
        {
            ActivateItem(_gameSetupViewModelFactory.Create());
        }

        private void StartGamePlay(IGame game)
        {
            var gameboardViewModel = _gameboardViewModelFactory.Create(game);
            ActivateItem(gameboardViewModel);
        }
    }
}