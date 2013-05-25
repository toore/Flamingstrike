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
        private readonly IGameEventAggregator _gameEventAggregator;

        public GameSettingsViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IGameEventAggregator gameEventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _gameEventAggregator = gameEventAggregator;
        }

        public IGameSettingsViewModel Create()
        {
            return new GameSettingsViewModel(_playerFactory, _playerTypes, _gameEventAggregator);
        }
    }
}