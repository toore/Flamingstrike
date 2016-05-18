using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Extensions;
using GuiWpf.Properties;
using GuiWpf.ViewModels.Messages;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Settings
{
    public class GameSettingsViewModel : ViewModelBase, IGameSettingsViewModel
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerRepository _playerRepository;

        public GameSettingsViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IPlayerRepository playerRepository, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
            _playerRepository = playerRepository;

            const int maxNumberOfPlayers = 6;
            Players = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(CreatePlayerSetupViewModel)
                .ToObservableCollection();
        }

        private PlayerSetupViewModel CreatePlayerSetupViewModel(int playerIndex)
        {
            return new PlayerSetupViewModel(_playerTypes)
            {
                Name = string.Format(Resources.PLAYER, playerIndex + 1),
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