using Caliburn.Micro;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel
    {
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModel(IGameSetupViewModelFactory gameSetupViewModelFactory, IGameboardViewModelFactory gameboardViewModelFactory, IPlayerRepository playerRepository)
        {
            _gameSetupViewModelFactory = gameSetupViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playerRepository = playerRepository;

            MainViewModel = _gameSetupViewModelFactory.Create(Confirm);
        }

        private void Confirm(GameSetup gameSetup)
        {
            gameSetup.Players.Apply(_playerRepository.Add);

            StartNewGame();
        }

        private void StartNewGame()
        {
            MainViewModel = _gameboardViewModelFactory.Create();
        }

        private IMainGameViewViewModel _mainViewModel;
        public IMainGameViewViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}