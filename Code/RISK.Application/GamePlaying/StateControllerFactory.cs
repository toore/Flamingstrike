using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IStateControllerFactory
    {
        IStateController Create(IPlayer currentPlayer, Game game);
    }

    public class StateControllerFactory : IStateControllerFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public StateControllerFactory(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public IStateController Create(IPlayer currentPlayer, Game game)
        {
            return new StateController(_interactionStateFactory, currentPlayer, game);
        }
    }
}