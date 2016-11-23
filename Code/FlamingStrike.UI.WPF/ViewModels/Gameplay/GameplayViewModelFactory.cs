using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IGameplayViewModelFactory
    {
        IGameplayViewModel Create();
    }

    public class GameplayViewModelFactory : IGameplayViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerStatusViewModelFactory _playerStatusViewModelFactory;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public GameplayViewModelFactory(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IPlayerStatusViewModelFactory playerStatusViewModelFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _playerStatusViewModelFactory = playerStatusViewModelFactory;
        }

        public IGameplayViewModel Create()
        {
            return new GameplayViewModel(
                _interactionStateFactory,
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                _playerStatusViewModelFactory);
        }
    }
}