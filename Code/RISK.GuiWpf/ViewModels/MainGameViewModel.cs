using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class MainGameViewModel : ViewModelBase, IMainGameViewModel, IHandle<GameSetupMessage>, ILocationSelector
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
            StorePlayersInRepository(message.Players);

            StartNewGame();
        }

        private void StorePlayersInRepository(IEnumerable<IPlayer> players)
        {
            players.Apply(_playerRepository.Add);
        }

        private void StartNewGame()
        {
            var game = _gameFactory.Create(this);
            var gameboardViewModel = _gameboardViewModelFactory.Create(game);

            MainViewModel = gameboardViewModel;
        }

        public ILocation Select(SelectLocationParameter selectLocationParameter)
        {
            //TODO: Select from gui?
            return selectLocationParameter.AvailableLocations.First();
        }

        private IMainGameViewViewModel _mainViewModel;
        public IMainGameViewViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set { NotifyOfPropertyChange(value, () => MainViewModel, x => _mainViewModel = x); }
        }
    }
}