using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IPlayerStatusViewModelFactory
    {
        PlayerStatusViewModel Create(Player player);
    }

    public class PlayerStatusViewModelFactory : IPlayerStatusViewModelFactory
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;

        public PlayerStatusViewModelFactory(IPlayerUiDataRepository playerUiDataRepository)
        {
            _playerUiDataRepository = playerUiDataRepository;
        }

        public PlayerStatusViewModel Create(Player player)
        {
            var name = player.Name;
            var playerUiData = _playerUiDataRepository.Get(name);

            return new PlayerStatusViewModel(name, playerUiData.Color, player.NumberOfCards);
        }
    }
}