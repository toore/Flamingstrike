using RISK.Application;
using RISK.Application.GamePlay;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateControllerFactory
    {
        IStateController Create(IPlayerId currentPlayerId, Game game);
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public StateControllerFactory(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public IStateController Create(IPlayerId currentPlayerId, Game game)
        {
            return new StateController(_interactionStateFactory, currentPlayerId, game);
        }
    }
}