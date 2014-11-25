using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<SetupGameMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameInitializationViewModelFactory _gameInitializationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel()
            : this(new Root()) { }

        private MainGameViewModel(Root root)
            : this(root.GameInitializationViewModelFactory, root.GameboardViewModelFactory, root.GameSetupViewModelFactory)
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

        public void Handle(StartGameplayMessage gameplaySetupMessage)
        {
            StartGamePlay(gameplaySetupMessage.Game);
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