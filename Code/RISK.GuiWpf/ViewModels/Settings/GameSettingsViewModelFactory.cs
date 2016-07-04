using Caliburn.Micro;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameInitializationViewModelFactory
    {
        IGamePreparationViewModel Create();
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

        public IGamePreparationViewModel Create()
        {
            return new GamePreparationViewModel(_playerFactory, _playerTypes, _playerRepository, _eventAggregator);
        }
    }
}