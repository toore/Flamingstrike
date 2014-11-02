using System.Threading.Tasks;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactoryWorker
    {
        void Run(IGameInitializerLocationSelector gameInitializerLocationSelector, IGameInitializerNotifier gameInitializerNotifier);
    }

    public class GameFactoryWorker : IGameFactoryWorker
    {
        private readonly IGameFactory _gameFactory;

        public GameFactoryWorker(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Run(IGameInitializerLocationSelector gameInitializerLocationSelector, IGameInitializerNotifier gameInitializerNotifier)
        {
            Task.Run(() =>
            {
                var game = _gameFactory.Create(gameInitializerLocationSelector);
                gameInitializerNotifier.InitializationFinished(game);
            }
            );
        }
    }
}