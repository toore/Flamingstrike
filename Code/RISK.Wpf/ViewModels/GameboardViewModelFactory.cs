using GuiWpf.ViewModels.Gameplay;

namespace GuiWpf.ViewModels
{
    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IGameEngineFactory _gameEngineFactory;

        public GameboardViewModelFactory(IGameEngineFactory gameEngineFactory)
        {
            _gameEngineFactory = gameEngineFactory;
        }

        public IGameboardViewModel Create()
        {
            return new GameboardViewModel(_gameEngineFactory.Create());
        }
    }
}