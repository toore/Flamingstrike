using System.Threading.Tasks;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactoryWorker
    {
        void Run(ITerritorySelector territorySelector, IGameInitializerNotifier gameInitializerNotifier);
    }

    public class GameFactoryWorker : IGameFactoryWorker
    {
        private readonly IGameFactory _gameFactory;

        public GameFactoryWorker(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Run(ITerritorySelector territorySelector, IGameInitializerNotifier gameInitializerNotifier)
        {
            Task.Run(() =>
            {
                var game = _gameFactory.Create(territorySelector);
                gameInitializerNotifier.InitializationFinished(game);
            }
            );
        }
    }
}