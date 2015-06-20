using RISK.Application;
using RISK.Application.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateControllerFactory
    {
        IStateController Create(IPlayer currentPlayer, IGame game);
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public StateControllerFactory(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public IStateController Create(IPlayer currentPlayer, IGame game)
        {
            return new StateController(_interactionStateFactory, currentPlayer, game);
        }
    }
}