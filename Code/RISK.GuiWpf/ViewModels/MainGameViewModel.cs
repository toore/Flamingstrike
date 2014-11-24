using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : Screen, IGameSettingStateConductor, IHandle<GameSetupMessage>, IHandle<NewGameMessage>
    {
        private readonly IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel()
            : this(new Root())
        { }

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
            get { return "CONQUER THE WORLD - YARC (Yet Another Risk Clone)!"; }
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
            MainViewModel = _gameSettingsViewModelFactory.Create();
        }

        private void StartGame()
        {
            var gameSetupViewModel = _gameSetupViewModelFactory.Create(this);
            MainViewModel = gameSetupViewModel;

            gameSetupViewModel.StartSetup();
        }

        public void StartGamePlay(IGame game)
        {
            MainViewModel = _gameboardViewModelFactory.Create(game);
        }

        private IMainViewModel _mainViewModel;
        public IMainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { this.NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}