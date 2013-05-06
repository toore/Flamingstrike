using System.Collections.Generic;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel
    {
        private readonly IGameSettingsViewModel _gameSettingsViewModel;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel(IGameSettingsViewModel gameSettingsViewModel, IGameboardViewModelFactory gameboardViewModelFactory, IPlayerRepository playerRepository, IGameSetupViewModelFactory gameSetupViewModelFactory)
        {
            _gameSettingsViewModel = gameSettingsViewModel;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playerRepository = playerRepository;
            _gameSetupViewModelFactory = gameSetupViewModelFactory;

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
            MainViewModel = _gameSetupViewModelFactory.Create(this);
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