using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IDispatcherWrapper _dispatcherWrapper;
        private readonly IUserInputRequest _userInputRequest;

        public GameSetupViewModelFactory(IWorldMapViewModelFactory worldMapViewModelFactory, IGameFactoryWorker gameFactoryWorker, IDispatcherWrapper dispatcherWrapper, IUserInputRequest userInputRequest)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _dispatcherWrapper = dispatcherWrapper;
            _userInputRequest = userInputRequest;
        }

        public IGameSetupViewModel Create(IGameStateConductor gameStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _dispatcherWrapper, gameStateConductor, _userInputRequest);
        }
    }
}