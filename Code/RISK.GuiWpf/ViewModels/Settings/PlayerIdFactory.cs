using RISK.Application;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerIdFactory
    {
        IPlayer Create(PlayerSetupViewModel playerSetupViewModel);
    }

    public class PlayerIdFactory : IPlayerIdFactory
    {
        public IPlayer Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new Player(playerSetupViewModel.Name);
        }
    }
}