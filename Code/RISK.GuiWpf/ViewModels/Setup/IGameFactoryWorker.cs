using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactoryWorker
    {
        void BeginInvoke(IGameFactoryWorkerCallback callback);
    }

    public interface IGameFactoryWorkerCallback
    {
        ILocation GetLocationCallback(ILocationSelectorParameter locationSelectorParameter);
        void OnFinished(IGame game);
    }
}