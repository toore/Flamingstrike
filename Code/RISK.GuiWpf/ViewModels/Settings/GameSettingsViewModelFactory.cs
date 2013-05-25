namespace GuiWpf.ViewModels.Settings
{
    public interface IGameSettingsViewModelFactory
    {
        IGameSettingsViewModel Create();
    }

    public class GameSettingsViewModelFactory : IGameSettingsViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IGameSettingsEventAggregator _gameSettingsEventAggregator;

        public GameSettingsViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IGameSettingsEventAggregator gameSettingsEventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _gameSettingsEventAggregator = gameSettingsEventAggregator;
        }

        public IGameSettingsViewModel Create()
        {
            return new GameSettingsViewModel(_playerFactory, _playerTypes, _gameSettingsEventAggregator);
        }
    }
}