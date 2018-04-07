using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IPlayerStatusViewModelFactory
    {
        PlayerStatusViewModel Create(IPlayerGameData playerGameData);
    }

    public class PlayerStatusViewModelFactory : IPlayerStatusViewModelFactory
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public PlayerStatusViewModelFactory(IPlayerUiDataRepository playerUiDataRepository)
        {
            _playerUiDataRepository = playerUiDataRepository;
        }

        public PlayerStatusViewModel Create(IPlayerGameData playerGameData)
        {
            var player = playerGameData.PlayerName;
            var playerUiData = _playerUiDataRepository.Get((string)player);

            return new PlayerStatusViewModel((string)player, playerUiData.Color, playerGameData.Cards.Count);
        }
    }
}