using RISK.Core;

namespace GuiWpf.ViewModels.Settings
{
    public interface IPlayerFactory
    {
        IPlayer Create(PlayerSetupViewModel playerSetupViewModel);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new Player(playerSetupViewModel.Name);
        }
    }
}