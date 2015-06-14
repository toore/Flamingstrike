using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Extensions;
using GuiWpf.ViewModels.Messages;
using RISK.Application;
using RISK.Application.GamePlay;

namespace GuiWpf.ViewModels.Settings
{
    public class GameSettingsViewModel : ViewModelBase, IGameSettingsViewModel
    {
        private readonly IPlayerIdFactory _playerIdFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerRepository _playerRepository;

        public GameSettingsViewModel(IPlayerIdFactory playerIdFactory, IPlayerTypes playerTypes, IPlayerRepository playerRepository, IEventAggregator eventAggregator)
        {
            _playerIdFactory = playerIdFactory;
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
            _playerRepository = playerRepository;

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

        public ObservableCollection<PlayerSetupViewModel> Players { get; }

        public bool CanConfirm => GetEnabledPlayers().Count() > 1;

        public void Confirm()
        {
            _playerRepository.Clear();

            foreach (var player in CreatePlayers())
            {
                _playerRepository.Add(player);
            }

            _eventAggregator.PublishOnUIThread(new SetupGameMessage());
        }

        private IEnumerable<IPlayerId> CreatePlayers()
        {
            return GetEnabledPlayers()
                .Select(_playerIdFactory.Create)
                .ToList();
        }

        private IEnumerable<PlayerSetupViewModel> GetEnabledPlayers()
        {
            return Players
                .Where(x => x.IsEnabled);
        }
    }
}