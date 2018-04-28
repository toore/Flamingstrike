using System.Linq;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
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
        private readonly IGameEngineClientProxy _gameEngineClientProxy;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public MainGameViewModel()
            : this(new CompositionRoot()) {}

        protected MainGameViewModel(CompositionRoot compositionRoot)
            : this(
                compositionRoot.PlayerUiDataRepository,
                compositionRoot.GamePreparationViewModelFactory,
                compositionRoot.GameplayViewModelFactory,
                compositionRoot.AlternateGameSetupViewModelFactory,
                compositionRoot.GameEngineClientProxy)
        {
            compositionRoot.EventAggregator.Subscribe(this);
        }

        protected MainGameViewModel(
            IPlayerUiDataRepository playerUiDataRepository,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameEngineClientProxy gameEngineClientProxy)
        {
            _playerUiDataRepository = playerUiDataRepository;
            _gamePreparationViewModelFactory = gamePreparationViewModelFactory;
            _gameplayViewModelFactory = gameplayViewModelFactory;
            _alternateGameSetupViewModelFactory = alternateGameSetupViewModelFactory;
            _gameEngineClientProxy = gameEngineClientProxy;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeNewGame();
        }

        public override string DisplayName
        {
            get { return "Flaming Strike"; }
            set {}
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

            _gameEngineClientProxy.Setup(gameSetupViewModel, players);

            ActivateItem(gameSetupViewModel);
        }

        private void StartGamePlay(IGamePlaySetup gamePlaySetup)
        {
            IGameplayViewModel gameplayViewModel = _gameplayViewModelFactory.Create();

            _gameEngineClientProxy.StartGame(gameplayViewModel, gamePlaySetup);

            ActivateItem(gameplayViewModel);
        }
    }
}