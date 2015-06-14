using Caliburn.Micro;
using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameInitializationViewModelFactory
    {
        IGameSettingsViewModel Create();
    }

    public class GameInitializationViewModelFactory : IGameInitializationViewModelFactory
    {
        private readonly IPlayerIdFactory _playerIdFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEventAggregator _eventAggregator;

        public GameInitializationViewModelFactory(IPlayerIdFactory playerIdFactory, IPlayerTypes playerTypes, IPlayerRepository playerRepository, IEventAggregator eventAggregator)
        {
            _playerIdFactory = playerIdFactory;
            _playerTypes = playerTypes;
            _playerRepository = playerRepository;
            _eventAggregator = eventAggregator;
        }

        public IGameSettingsViewModel Create()
        {
            return new GameSettingsViewModel(_playerIdFactory, _playerTypes, _playerRepository, _eventAggregator);
        }
    }
}