using System.Collections.Generic;
using Caliburn.Micro;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel, IHandle<GameSetupMessage>
    {
        private readonly IGameFactory _gameFactory;
        private readonly IGameSettingsViewModel _gameSettingsViewModel;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameSetupViewModel _gameSetupViewModel;

        public MainGameViewModel(IGameFactory gameFactory, IGameSettingsViewModel gameSettingsViewModel, IGameboardViewModelFactory gameboardViewModelFactory, IPlayerRepository playerRepository, IGameSetupViewModel gameSetupViewModel)
        {
            _gameFactory = gameFactory;
            _gameSettingsViewModel = gameSettingsViewModel;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playerRepository = playerRepository;
            _gameSetupViewModel = gameSetupViewModel;

            MainViewModel = _gameSettingsViewModel;
        }

        public void Handle(GameSetupMessage message)
        {
            StorePlayersInRepository(message.Players);

            StartNewGame();
        }

        private void StorePlayersInRepository(IEnumerable<IPlayer> players)
        {
            players.Apply(_playerRepository.Add);
        }

        private void StartNewGame()
        {
            MainViewModel = _gameSetupViewModel;

            var game = _gameFactory.Create(_gameSetupViewModel);
            var gameboardViewModel = _gameboardViewModelFactory.Create(game);

            MainViewModel = gameboardViewModel;
        }

        private IMainGameViewViewModel _mainViewModel;
        public IMainGameViewViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}