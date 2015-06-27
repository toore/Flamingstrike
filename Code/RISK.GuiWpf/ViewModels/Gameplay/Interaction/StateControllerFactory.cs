using RISK.Application.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateControllerFactory
    {
        IStateController Create(IGame game);
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        public IStateController Create(IGame game)
        {
            return new StateController(game);
        }
    }
}