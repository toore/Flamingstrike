using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IDispatcherWrapper _dispatcherWrapper;
        private readonly IUserInputRequestHandler _userInputRequestHandler;

        public GameSetupViewModelFactory(IWorldMapViewModelFactory worldMapViewModelFactory, IGameFactoryWorker gameFactoryWorker, IDispatcherWrapper dispatcherWrapper, IUserInputRequestHandler userInputRequestHandler)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _dispatcherWrapper = dispatcherWrapper;
            _userInputRequestHandler = userInputRequestHandler;
        }

        public IGameSetupViewModel Create(IGameStateConductor gameStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _dispatcherWrapper, gameStateConductor, _userInputRequestHandler);
        }
    }
}