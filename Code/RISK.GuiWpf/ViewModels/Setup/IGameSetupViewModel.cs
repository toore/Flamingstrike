using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainGameViewViewModel, IGameFactoryWorkerCallback
    {
        void SelectLocation(ILocation location);
    }
}