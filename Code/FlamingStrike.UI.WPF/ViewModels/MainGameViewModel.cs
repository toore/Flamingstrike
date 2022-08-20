using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IGameEngineClient _gameEngineClient;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public MainGameViewModel()
            : this(new CompositionRoot()) {}

        protected MainGameViewModel(CompositionRoot compositionRoot)
            : this(
                compositionRoot.PlayerUiDataRepository,
                compositionRoot.GamePreparationViewModelFactory,
                compositionRoot.GameplayViewModelFactory,
                compositionRoot.AlternateGameSetupViewModelFactory,
                compositionRoot.GameEngineClient)
        {
            compositionRoot.EventAggregator.SubscribeOnPublishedThread(this);
        }

        protected MainGameViewModel(
            IPlayerUiDataRepository playerUiDataRepository,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameEngineClient gameEngineClient)
        {
            _playerUiDataRepository = playerUiDataRepository;
            _gamePreparationViewModelFactory = gamePreparationViewModelFactory;
            _gameplayViewModelFactory = gameplayViewModelFactory;
            _alternateGameSetupViewModelFactory = alternateGameSetupViewModelFactory;
            _gameEngineClient = gameEngineClient;
        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);

            await InitializeNewGameAsync();
        }

        public override string DisplayName
        {
            get { return "Flaming Strike"; }
            set {}
        }

        public async Task HandleAsync(NewGameMessage newGameMessage, CancellationToken cancellationToken)
        {
            await InitializeNewGameAsync();
        }

        public async Task HandleAsync(StartGameSetupMessage startGameSetupMessage, CancellationToken cancellationToken)
        {
            await StartGameSetupAsync();
        }

        public async Task HandleAsync(StartGameplayMessage startGameplayMessage, CancellationToken cancellationToken)
        {
            await StartGamePlayAsync(startGameplayMessage.GamePlaySetup);
        }

        private async Task InitializeNewGameAsync()
        {
            var gamePreparationViewModel = _gamePreparationViewModelFactory.Create();

            await ActivateItemAsync(gamePreparationViewModel);
        }

        private async Task StartGameSetupAsync()
        {
            var players = _playerUiDataRepository.GetAll().Select(x => x.Player).ToList();
            var gameSetupViewModel = _alternateGameSetupViewModelFactory.Create();

            await ActivateItemAsync(gameSetupViewModel);

            _gameEngineClient.Setup(players);
        }

        private async Task StartGamePlayAsync(IGamePlaySetup gamePlaySetup)
        {
            var gameplayViewModel = _gameplayViewModelFactory.Create();

            await ActivateItemAsync(gameplayViewModel);

            _gameEngineClient.StartGame(gamePlaySetup);
        }
    }
}