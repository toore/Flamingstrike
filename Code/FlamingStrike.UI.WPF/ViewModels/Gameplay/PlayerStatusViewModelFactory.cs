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
            var player = playerGameData.Player;
            var playerUiData = _playerUiDataRepository.Get(player);

            return new PlayerStatusViewModel(player.Name, playerUiData.Color, playerGameData.Cards.Count);
        }
    }
}