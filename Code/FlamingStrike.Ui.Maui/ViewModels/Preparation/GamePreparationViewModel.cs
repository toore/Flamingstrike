using Caliburn.Micro;
using CommunityToolkit.Mvvm.Input;
using FlamingStrike.Maui.ViewModels.Messages;

namespace FlamingStrike.Maui.ViewModels.Preparation
{
    public interface IGamePreparationViewModel
    {
        IList<GamePreparationPlayerViewModel> PotentialPlayers { get; }
        Task ConfirmAsync();
    }

    public partial class GamePreparationViewModel : ViewModelBase, IGamePreparationViewModel
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
                    Name = $"Player {playerIndex + 1}",
                    OnIsEnabledChanged = OnEnabledPlayerChanged,
                    Color = color
                };
        }

        private void OnEnabledPlayerChanged()
        {
            //NotifyOfPropertyChange(() => CanConfirm);
        }

        public IList<GamePreparationPlayerViewModel> PotentialPlayers { get; }

        public bool CanConfirm => GetPlayers().Count() > 1;

        [RelayCommand]
        public async Task ConfirmAsync()
        {
            _playerUiDataRepository.Clear();

            foreach (var player in GetPlayers())
            {
                _playerUiDataRepository.Add(new PlayerUiData(player.Name, player.Color));
            }

            await _eventAggregator.PublishOnUIThreadAsync(new StartGameSetupMessage());
        }

        private IEnumerable<GamePreparationPlayerViewModel> GetPlayers()
        {
            return PotentialPlayers
                .Where(x => x.IsEnabled);
        }
    }
}