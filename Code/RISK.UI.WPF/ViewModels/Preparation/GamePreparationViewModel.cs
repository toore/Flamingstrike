using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.ViewModels.Messages;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public interface IGamePreparationViewModel : IMainViewModel
    {
        IList<GamePreparationPlayerViewModel> Players { get; }
        void Confirm();
    }

    public class GamePreparationViewModel : ViewModelBase, IGamePreparationViewModel
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public GamePreparationViewModel(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IPlayerUiDataRepository playerUiDataRepository, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
            _playerUiDataRepository = playerUiDataRepository;

            var playerColors = new[]
                {
                    Colors.Black,
                    Colors.WhiteSmoke,
                    Colors.CornflowerBlue,
                    Colors.MediumPurple,
                    Colors.DarkGoldenrod,
                    Colors.MediumSeaGreen
                };

            const int maxNumberOfPlayers = 6;
            Players = Enumerable.Range(0, maxNumberOfPlayers)
                .Select(i => CreateGamePreparationPlayerViewModel(i, new PlayerColor(playerColors[i])))
                .ToList();
        }

        private GamePreparationPlayerViewModel CreateGamePreparationPlayerViewModel(int playerIndex, PlayerColor playerColor)
        {
            return new GamePreparationPlayerViewModel(_playerTypes, playerColor)
                {
                    Name = string.Format(Resources.PLAYER_NUMBER, playerIndex + 1),
                    OnIsEnabledChanged = () => OnEnabledPlayerChanged()
                };
        }

        private void OnEnabledPlayerChanged()
        {
            NotifyOfPropertyChange(() => CanConfirm);
        }

        public IList<GamePreparationPlayerViewModel> Players { get; }

        public bool CanConfirm => GetEnabledPlayers().Count() > 1;

        public void Confirm()
        {
            _playerUiDataRepository.Clear();

            foreach (var player in GetEnabledPlayers())
            {
                _playerUiDataRepository.Add(
                    new PlayerUiData(_playerFactory.Create(player.Name), player.PlayerColor.FillColor));
            }

            _eventAggregator.PublishOnUIThread(new StartGameSetupMessage());
        }

        private IEnumerable<GamePreparationPlayerViewModel> GetEnabledPlayers()
        {
            return Players
                .Where(x => x.IsEnabled);
        }
    }

    public class PlayerColor
    {
        public Color FillColor { get; }

        public PlayerColor(Color fillColor)
        {
            FillColor = fillColor;
        }
    }
}