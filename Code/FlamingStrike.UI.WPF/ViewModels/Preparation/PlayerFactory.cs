using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
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