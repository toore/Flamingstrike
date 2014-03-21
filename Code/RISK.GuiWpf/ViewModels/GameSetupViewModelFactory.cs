using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IUserInteractionSynchronizer _userInteractionSynchronizer;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameFactoryWorker gameFactoryWorker,
            IUserInteractionSynchronizer userInteractionSynchronizer,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _userInteractionSynchronizer = userInteractionSynchronizer;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameSetupViewModel Create(IGameSettingStateConductor gameSettingStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, gameSettingStateConductor, _userInteractionSynchronizer, _dialogManager, _eventAggregator);
        }
    }
}