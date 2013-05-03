using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerFactory
    {
        IPlayer Create(PlayerSetupViewModel playerSetupViewModel);
    }
}