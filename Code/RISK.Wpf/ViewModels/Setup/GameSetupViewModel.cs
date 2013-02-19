using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel
    {
        private readonly Action<GameSetup> _confirm;
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;

        public GameSetupViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, Action<GameSetup> confirm)
        {
            _confirm = confirm;
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;

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
            var gameSetupResult = new GameSetup
                {
                    Players = CreatePlayers()
                };

            _confirm(gameSetupResult);
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