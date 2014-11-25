using Caliburn.Micro;
using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameInitializationViewModelFactory
    {
        IGameInitializationViewModel Create();
    }

    public class GameInitializationViewModelFactory : IGameInitializationViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IPlayerRepository _playerRepository;
        private readonly IEventAggregator _eventAggregator;

        public GameInitializationViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IPlayerRepository playerRepository, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _playerRepository = playerRepository;
            _eventAggregator = eventAggregator;
        }

        public IGameInitializationViewModel Create()
        {
            return new GameInitializationViewModel(_playerFactory, _playerTypes, _playerRepository, _eventAggregator);
        }
    }
}