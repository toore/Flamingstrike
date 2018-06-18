using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.ViewModels.Messages;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
{
    public interface IGamePreparationViewModel : IMainViewModel
    {
        IList<GamePreparationPlayerViewModel> PotentialPlayers { get; }
        void Confirm();
    }

    public class GamePreparationViewModel : ViewModelBase, IGamePreparationViewModel
    {
        private readonly IPlayerTypes _playerTypes;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public GamePreparationViewModel(IPlayerTypes playerTypes, IPlayerUiDataRepository playerUiDataRepository, IEventAggregator eventAggregator)
        {
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
            _playerUiDataRepository = playerUiDataRepository;

            var playerColors = new[]
                {
                    Color.FromRgb(0x32, 0x32, 0x32),
                    Colors.DarkOrange,
                    Colors.DarkRed,
                    Colors.Purple,
                    Colors.DarkGreen,
                    Color.FromRgb(0xC8, 0xC8, 0xC8)
                };

            const int maxNumberOfPlayers = 6;
            PotentialPlayers = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(i => CreateGamePreparationPlayerViewModel(i, playerColors[i]))
                .ToList();
        }

        private GamePreparationPlayerViewModel CreateGamePreparationPlayerViewModel(int playerIndex, Color color)
        {
            return new GamePreparationPlayerViewModel(_playerTypes)
                {
                    Name = string.Format(Resources.PLAYER_NUMBER, playerIndex + 1),
                    OnIsEnabledChanged = OnEnabledPlayerChanged,
                    Color = color
                };
        }

        private void OnEnabledPlayerChanged()
        {
            NotifyOfPropertyChange(() => CanConfirm);
        }

        public IList<GamePreparationPlayerViewModel> PotentialPlayers { get; }

        public bool CanConfirm => GetPlayers().Count() > 1;

        public void Confirm()
        {
            _playerUiDataRepository.Clear();

            foreach (var player in GetPlayers())
            {
                _playerUiDataRepository.Add(new PlayerUiData(player.Name, player.Color));
            }

            _eventAggregator.PublishOnUIThread(new StartGameSetupMessage());
        }

        private IEnumerable<GamePreparationPlayerViewModel> GetPlayers()
        {
            return PotentialPlayers
                .Where(x => x.IsEnabled);
        }
    }
}