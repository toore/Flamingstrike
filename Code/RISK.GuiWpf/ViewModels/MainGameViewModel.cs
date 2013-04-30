using Caliburn.Micro;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel, IHandle<GameSetupMessage>
    {
        private readonly IGameFactory _gameFactory;
        private readonly IGameSetupViewModel _gameSetupViewModel;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModel(IGameFactory gameFactory, IGameSetupViewModel gameSetupViewModel, IGameboardViewModelFactory gameboardViewModelFactory, IPlayerRepository playerRepository)
        {
            _gameFactory = gameFactory;
            _gameSetupViewModel = gameSetupViewModel;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playerRepository = playerRepository;

            MainViewModel = _gameSetupViewModel;
        }

        public void Handle(GameSetupMessage message)
        {
            message.Players.Apply(_playerRepository.Add);

            StartNewGame();
        }

        private void StartNewGame()
        {
            var game = _gameFactory.Create();

            MainViewModel = _gameboardViewModelFactory.Create(game);
        }

        private IMainGameViewViewModel _mainViewModel;
        public IMainGameViewViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}