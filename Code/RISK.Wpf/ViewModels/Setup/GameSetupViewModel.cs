using System;
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

        public GameSetupViewModel(Action<GameSetup> confirm)
        {
            _confirm = confirm;
            const int maxNumberOfPlayers = 6;

            Players = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(x => new PlayerSetupViewModel())
                .ToObservableCollection();
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
                .Select(Create)
                .ToList();
        }

        private IPlayer Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new HumanPlayer("");
        }
    }
}