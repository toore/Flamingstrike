using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.Play;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameAdapterFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;

        public GameAdapterFactory(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
        }

        public GameAdapter Create(Game game)
        {
            return new GameAdapter(_interactionStateFactory, _stateControllerFactory, game);
        }
    }
}