using Caliburn.Micro;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public interface IGamePreparationViewModelFactory
    {
        IGamePreparationViewModel Create();
    }

    public class GamePreparationViewModelFactory : IGamePreparationViewModelFactory
    {
        private readonly IPlayerFactory _playerFactory;
        private readonly IPlayerTypes _playerTypes;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IEventAggregator _eventAggregator;

        public GamePreparationViewModelFactory(IPlayerFactory playerFactory, IPlayerTypes playerTypes, IPlayerUiDataRepository playerUiDataRepository, IEventAggregator eventAggregator)
        {
            _playerFactory = playerFactory;
            _playerTypes = playerTypes;
            _playerUiDataRepository = playerUiDataRepository;
            _eventAggregator = eventAggregator;
        }

        public IGamePreparationViewModel Create()
        {
            return new GamePreparationViewModel(_playerFactory, _playerTypes, _playerUiDataRepository, _eventAggregator);
        }
    }
}