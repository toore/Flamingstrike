using Caliburn.Micro;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
{
    public interface IGamePreparationViewModelFactory
    {
        IGamePreparationViewModel Create();
    }

    public class GamePreparationViewModelFactory : IGamePreparationViewModelFactory
    {
        private readonly IPlayerTypes _playerTypes;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IEventAggregator _eventAggregator;

        public GamePreparationViewModelFactory(IPlayerTypes playerTypes, IPlayerUiDataRepository playerUiDataRepository, IEventAggregator eventAggregator)
        {
            _playerTypes = playerTypes;
            _playerUiDataRepository = playerUiDataRepository;
            _eventAggregator = eventAggregator;
        }

        public IGamePreparationViewModel Create()
        {
            return new GamePreparationViewModel(_playerTypes, _playerUiDataRepository, _eventAggregator);
        }
    }
}