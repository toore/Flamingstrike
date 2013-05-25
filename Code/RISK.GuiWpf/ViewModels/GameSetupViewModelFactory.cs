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
        private readonly IResourceManagerWrapper _resourceManagerWrapper;
        private readonly IDialogManager _dialogManager;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory, 
            IGameFactoryWorker gameFactoryWorker, 
            IUserInteractionSynchronizer userInteractionSynchronizer, 
            IResourceManagerWrapper resourceManagerWrapper,
            IDialogManager dialogManager)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _userInteractionSynchronizer = userInteractionSynchronizer;
            _resourceManagerWrapper = resourceManagerWrapper;
            _dialogManager = dialogManager;
        }

        public IGameSetupViewModel Create(IGameStateConductor gameStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, gameStateConductor, _userInteractionSynchronizer, _dialogManager);
        }
    }
}