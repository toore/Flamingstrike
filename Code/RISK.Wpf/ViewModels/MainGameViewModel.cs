using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel
    {
        private readonly IGameSetupViewModel _gameSetupViewModel;
        private readonly IGameboardViewModel _gameboardViewModel;
        private readonly IPlayerRepository _playerRepository;

        //Event aggregator
        public MainGameViewModel(IGameSetupViewModel gameSetupViewModel, IGameboardViewModel gameboardViewModel, IPlayerRepository playerRepository)
        {
            _gameSetupViewModel = gameSetupViewModel;
            _gameboardViewModel = gameboardViewModel;
            _playerRepository = playerRepository;

            MainViewModel = _gameSetupViewModel;
        }

        private void Confirm(GameSetup gameSetup)
        {
            gameSetup.Players.Apply(_playerRepository.Add);

            StartNewGame();
        }

        private void StartNewGame()
        {
            MainViewModel = _gameboardViewModel;
        }

        private IMainGameViewViewModel _mainViewModel;
        public IMainGameViewViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}