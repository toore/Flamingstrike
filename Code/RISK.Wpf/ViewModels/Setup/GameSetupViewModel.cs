using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IGameSetupEventAggregator _gameSetupEventAggregator;

        public GameSetupViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IGameSetupEventAggregator gameSetupEventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _gameSetupEventAggregator = gameSetupEventAggregator;

            const int maxNumberOfPlayers = 6;
            Players = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(CreatePlayerSetupViewModel)
                .ToObservableCollection();
        }

        private PlayerSetupViewModel CreatePlayerSetupViewModel(int playerNumber)
        {
            return new PlayerSetupViewModel(_playerTypes)
                {
                    Name = "Player " + (playerNumber + 1),
                    OnIsEnabledChanged = () => OnEnabledPlayerChanged()
                };
        }

        private void OnEnabledPlayerChanged()
        {
            NotifyOfPropertyChange(() => CanConfirm);
        }

        public ObservableCollection<PlayerSetupViewModel> Players { get; private set; }

        public bool CanConfirm
        {
            get { return GetEnabledPlayers().Count() > 1; }
        }

        public void Confirm()
        {
            var gameSetup = new GameSetup
                {
                    Players = CreatePlayers()
                };

            _gameSetupEventAggregator.Publish(gameSetup);
        }

        private IEnumerable<IPlayer> CreatePlayers()
        {
            return GetEnabledPlayers()
                .Select(_playerFactory.Create)
                .ToList();
        }

        private IEnumerable<PlayerSetupViewModel> GetEnabledPlayers()
        {
            return Players
                .Where(x => x.IsEnabled);
        }
    }

    public interface IGameSetupEventAggregator : IEventAggregator {}

    public class GameSetupEventAggregator : EventAggregator, IGameSetupEventAggregator {}
}