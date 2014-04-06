using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Domain;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel, IHaveDisplayName
    {
        private readonly IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayersInitializer _playersInitializer;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel(IGameSettingsViewModelFactory gameSettingsViewModelFactory, IGameboardViewModelFactory gameboardViewModelFactory, IPlayersInitializer playersInitializer, IGameSetupViewModelFactory gameSetupViewModelFactory)
        {
            _gameSettingsViewModelFactory = gameSettingsViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playersInitializer = playersInitializer;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;

            StartNewGame();
        }

        public string DisplayName
        {
            get { return "CONQUER THE WORLD"; }
            set { }
        }

        public void Handle(GameSetupMessage message)
        {
            _playersInitializer.SetPlayers(message.Players);

            StartGame();
        }

        public void Handle(NewGameMessage message)
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
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}