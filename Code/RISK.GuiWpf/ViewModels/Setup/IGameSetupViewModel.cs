using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainViewModel, IGameFactoryWorkerCallback
    {
        void SelectLocation(ILocation location);
    }
}