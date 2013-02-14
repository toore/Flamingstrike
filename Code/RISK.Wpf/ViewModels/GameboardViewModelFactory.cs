using GuiWpf.ViewModels.Gameboard;

namespace GuiWpf.ViewModels
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IGameEngineFactory _gameEngineFactory;

        public GameboardViewModelFactory(IGameEngineFactory gameEngineFactory)
        {
            _gameEngineFactory = gameEngineFactory;
        }

        public IMainGameViewViewModel Create()
        {
            return new GameboardViewModel(_gameEngineFactory.Create());
        }
    }
}