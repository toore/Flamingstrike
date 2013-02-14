using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public interface IPlayerFactory
    {
        IPlayer Create(PlayerSetupViewModel playerSetupViewModel);
    }
}