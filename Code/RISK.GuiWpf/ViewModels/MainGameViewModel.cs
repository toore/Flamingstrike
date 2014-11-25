using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Conductor<IMainViewModel>, IGameSettingStateConductor, IHandle<GameSetupMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel()
            : this(new Root()) {}

        private MainGameViewModel(Root root)
            : this(root.GameSettingsViewModelFactory, root.GameboardViewModelFactory, root.GameSetupViewModelFactory)
        {
            root.EventAggregator.Subscribe(this);
        }

        public MainGameViewModel(
            IGameSettingsViewModelFactory gameSettingsViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IGameSetupViewModelFactory gameSetupViewModelFactory)
        {
            _gameSettingsViewModelFactory = gameSettingsViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            StartNewGame();
        }

        public override string DisplayName
        {
            get { return "Yarc"; }
            set { }
        }

        public void Handle(GameSetupMessage gameSetupMessage)
        {
            StartGame();
        }

        public void Handle(NewGameMessage newGameMessage)
        {
            StartNewGame();
        }

        private void StartNewGame()
        {
            ActivateItem(_gameSettingsViewModelFactory.Create());
        }

        private void StartGame()
        {
            ActivateItem(_gameSetupViewModelFactory.Create(this));
        }

        public void StartGamePlay(IGame game)
        {
            ActivateItem(_gameboardViewModelFactory.Create(game));
        }
    }
}