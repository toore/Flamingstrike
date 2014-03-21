using Caliburn.Micro;

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
        private readonly IEventAggregator _eventAggregator;

        public GameSettingsViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _eventAggregator = eventAggregator;
        }

        public IGameSettingsViewModel Create()
        {
            return new GameSettingsViewModel(_playerFactory, _playerTypes, _eventAggregator);
        }
    }
}