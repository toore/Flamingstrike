using RISK.GameEngine;

namespace GuiWpf.ViewModels.Preparation
{
    public interface IPlayerFactory
    {
        IPlayer Create(GamePreparationPlayerViewModel gamePreparationPlayerViewModel);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer Create(GamePreparationPlayerViewModel gamePreparationPlayerViewModel)
        {
            return new Player(gamePreparationPlayerViewModel.Name);
        }
    }
}