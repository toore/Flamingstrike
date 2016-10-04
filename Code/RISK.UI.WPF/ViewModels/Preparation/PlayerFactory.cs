using RISK.GameEngine;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public interface IPlayerFactory
    {
        IPlayer Create(string name);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(string name)
        {
            return new Player(name);
        }
    }
}