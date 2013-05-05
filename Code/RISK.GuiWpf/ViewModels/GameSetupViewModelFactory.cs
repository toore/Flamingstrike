using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IInputRequestHandler _inputRequestHandler;

        public GameSetupViewModelFactory(IWorldMapViewModelFactory worldMapViewModelFactory, IGameFactoryWorker gameFactoryWorker, IInputRequestHandler inputRequestHandler)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _inputRequestHandler = inputRequestHandler;
        }

        public IGameSetupViewModel Create(IGameStateConductor gameStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, gameStateConductor, _inputRequestHandler);
        }
    }
}