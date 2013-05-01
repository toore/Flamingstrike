using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GuiWpf.Extensions;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSettingsViewModel : ViewModelBase, IGameSettingsViewModel
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IGameSetupEventAggregator _gameSetupEventAggregator;

        public GameSettingsViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IGameSetupEventAggregator gameSetupEventAggregator)
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
            var gameSetup = new GameSetupMessage
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
}