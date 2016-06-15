using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateControllerFactory
    {
        IInteractionStateFsm Create(IGame game);
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        public IInteractionStateFsm Create(IGame game)
        {
            return new InteractionStateFsm(game);
        }
    }
}