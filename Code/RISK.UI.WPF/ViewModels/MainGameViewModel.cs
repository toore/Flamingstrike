using Caliburn.Micro;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;
using RISK.UI.WPF.ViewModels.AlternateSetup;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Messages;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<StartGameSetupMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameplayViewModelFactory _gameplayViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModel()
            : this(new Root()) {}

        protected MainGameViewModel(Root root)
            : this(
                root.PlayerRepository,
                root.AlternateGameSetupFactory,
                root.GamePreparationViewModelFactory,
                root.GameplayViewModelFactory,
                root.AlternateGameSetupViewModelFactory,
                root.GameFactory)
        {
            root.EventAggregator.Subscribe(this);
        }

        protected MainGameViewModel(
            IPlayerRepository playerRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameFactory gameFactory)
        {
            _playerRepository = playerRepository;
            _alternateGameSetupFactory = alternateGameSetupFactory;
            _gamePreparationViewModelFactory = gamePreparationViewModelFactory;
            _gameplayViewModelFactory = gameplayViewModelFactory;
            _alternateGameSetupViewModelFactory = alternateGameSetupViewModelFactory;
            _gameFactory = gameFactory;
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

        public void Handle(StartGameSetupMessage startGameSetupMessage)
        {
            StartGameSetup();
        }

        public void Handle(StartGameplayMessage startGameplayMessage)
        {
            StartGamePlay(startGameplayMessage.GamePlaySetup);
        }

        private void InitializeNewGame()
        {
            var gamePreparationViewModel = _gamePreparationViewModelFactory.Create();

            ActivateItem(gamePreparationViewModel);
        }

        private void StartGameSetup()
        {
            var players = _playerRepository.GetAll();
            var gameSetupViewModel = _alternateGameSetupViewModelFactory.Create();
            var alternateGameSetup = _alternateGameSetupFactory.Create(gameSetupViewModel, players);

            ActivateItem(gameSetupViewModel);
        }

        private void StartGamePlay(IGamePlaySetup gamePlaySetup)
        {
            var gameplayViewModel = _gameplayViewModelFactory.Create();
            var game = _gameFactory.Create(gameplayViewModel, gamePlaySetup);
            
            ActivateItem(gameplayViewModel);
        }
    }
}