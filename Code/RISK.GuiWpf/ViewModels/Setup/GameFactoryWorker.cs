using System.Threading.Tasks;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public class GameFactoryWorker : IGameFactoryWorker, ILocationSelector
    {
        private readonly IGameFactory _gameFactory;
        private IGameFactoryWorkerCallback _callback;

        public GameFactoryWorker(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void BeginInvoke(IGameFactoryWorkerCallback callback)
        {
            _callback = callback;

            Task.Run(() =>
                {
                    var game = _gameFactory.Create(this);
                    _callback.OnFinished(game);
                });
        }

        public ILocation GetLocation(ILocationSelectorParameter locationSelectorParameter)
        {
            return _callback.GetLocationCallback(locationSelectorParameter);
        }
    }
}