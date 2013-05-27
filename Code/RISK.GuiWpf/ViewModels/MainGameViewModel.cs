using System.Collections.Generic;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel, IHaveDisplayName
    {
        private readonly IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IPlayerProvider _playerProvider;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModel(IGameSettingsViewModelFactory gameSettingsViewModelFactory, IGameboardViewModelFactory gameboardViewModelFactory, IPlayerProvider playerProvider, IGameSetupViewModelFactory gameSetupViewModelFactory)
        {
            _gameSettingsViewModelFactory = gameSettingsViewModelFactory;
            _gameboardViewModelFactory = gameboardViewModelFactory;
            _playerProvider = playerProvider;
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
            //StorePlayersInRepository(message.Players);
            _playerProvider.All = message.Players;

            StartGame();
        }

        public void Handle(NewGameMessage message)
        {
            StartNewGame();
        }

        //private void StorePlayersInRepository(IEnumerable<IPlayer> players)
        //{
        //    players.Apply(_playerProvider.Add);
        //}

        private void StartNewGame()
        {
            MainViewModel = _gameSettingsViewModelFactory.Create();
        }

        private void StartGame()
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