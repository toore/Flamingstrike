using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerFactory
    {
        IPlayer Create(PlayerSetupViewModel playerSetupViewModel);
    }
}