using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.Play;
using RISK.Application.Setup;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<SetupGameMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameInitializationViewModelFactory _gameInitializationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IUserInteractorFactory _userInteractorFactory;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModel()
            : this(new Root()) {}

        protected MainGameViewModel(Root root)
            : this(
                root.PlayerRepository,
                root.AlternateGameSetupFactory,
                root.GameInitializationViewModelFactory,
                root.GameboardViewModelFactory,
                root.GameSetupViewModelFactory,
                root.UserInteractorFactory)
        {
            root.EventAggregator.Subscribe(this);
        }

        protected MainGameViewModel(
            IPlayerRepository playerRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGameInitializationViewModelFactory gameInitializationViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IGameSetupViewModelFactory gameSetupViewModelFactory,
            IUserInteractorFactory userInteractorFactory)
        {
            _playerRepository = playerRepository;
            _alternateGameSetupFactory = alternateGameSetupFactory;
            _gameInitializationViewModelFactory = gameInitializationViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;
            _userInteractorFactory = userInteractorFactory;
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
            StartGamePlay(startGameplayMessage.Game);
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

            var userInteractory = _userInteractorFactory.Create(gameSetupViewModel);
            alternateGameSetup.TerritoryResponder = userInteractory;

            ActivateItem(gameSetupViewModel);
        }

        private void StartGamePlay(IGame game)
        {
            var gameboardViewModel = _gameboardViewModelFactory.Create(game);

            ActivateItem(gameboardViewModel);
        }
    }
}