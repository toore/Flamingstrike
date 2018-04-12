using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IPlayerStatusViewModelFactory
    {
        PlayerStatusViewModel Create(IPlayer player);
    }

    public class PlayerStatusViewModelFactory : IPlayerStatusViewModelFactory
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public PlayerStatusViewModelFactory(IPlayerUiDataRepository playerUiDataRepository)
        {
            _playerUiDataRepository = playerUiDataRepository;
        }

        public PlayerStatusViewModel Create(IPlayer player)
        {
            var name = (string)player.PlayerName;
            var playerUiData = _playerUiDataRepository.Get(name);

            return new PlayerStatusViewModel(name, playerUiData.Color, player.Cards.Count);
        }
    }
}