using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerIdFactory
    {
        IPlayerId Create(PlayerSetupViewModel playerSetupViewModel);
    }

    public class PlayerIdFactory : IPlayerIdFactory
    {
        public IPlayerId Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new PlayerId(playerSetupViewModel.Name);
        }
    }
}