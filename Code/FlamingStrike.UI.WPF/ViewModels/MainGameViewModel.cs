using System.Linq;
using Caliburn.Micro;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IHandle<StartGameSetupMessage>, IHandle<StartGameplayMessage>, IHandle<NewGameMessage>
    {
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameplayViewModelFactory _gameplayViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public MainGameViewModel()
            : this(new Root()) { }

        protected MainGameViewModel(Root root)
            : this(
                root.PlayerUiDataRepository,
                root.AlternateGameSetupFactory,
                root.GamePreparationViewModelFactory,
                root.GameplayViewModelFactory,
                root.AlternateGameSetupViewModelFactory,
                root.GameFactory)
        {
            root.EventAggregator.Subscribe(this);
        }

        protected MainGameViewModel(
            IPlayerUiDataRepository playerUiDataRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameFactory gameFactory)
        {
            _playerUiDataRepository = playerUiDataRepository;
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
            var players = _playerUiDataRepository.GetAll().Select(x => x.Player).ToList();
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