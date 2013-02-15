﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupViewModel : IGameSetupViewModel
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
                    Name = "Player " + (playerNumber + 1)
                };
        }

        public ObservableCollection<PlayerSetupViewModel> Players { get; private set; }

        public void OnConfirm()
        {
            var gameSetupResult = new GameSetup
                {
                    Players = CreatePlayers()
                };

            _confirm(gameSetupResult);
        }

        private IEnumerable<IPlayer> CreatePlayers()
        {
            return Players
                .Where(x => x.IsEnabled)
                .Select(_playerFactory.Create)
                .ToList();
        }
    }
}