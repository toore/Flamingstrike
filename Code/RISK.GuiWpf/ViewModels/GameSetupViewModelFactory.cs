using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IInputRequestHandler _inputRequestHandler;
        private readonly IUserNotifier _userNotifier;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory, 
            IGameFactoryWorker gameFactoryWorker, 
            IInputRequestHandler inputRequestHandler, 
            IUserNotifier userNotifier, 
            IResourceManagerWrapper resourceManagerWrapper)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _inputRequestHandler = inputRequestHandler;
            _userNotifier = userNotifier;
            _resourceManagerWrapper = resourceManagerWrapper;
        }

        public IGameSetupViewModel Create(IGameStateConductor gameStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, gameStateConductor, _inputRequestHandler, _userNotifier, _resourceManagerWrapper);
        }
    }
}