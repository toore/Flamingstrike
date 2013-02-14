using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Setup
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(PlayerSetupViewModel playerSetupViewModel)
        {
            return new HumanPlayer("");
        }
    }
}