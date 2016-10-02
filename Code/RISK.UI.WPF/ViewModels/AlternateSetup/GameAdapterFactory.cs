using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.Application.Play;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameAdapterFactory
    {
        IGameAdapter Create(IGame game);
    }

    public class GameAdapterFactory : IGameAdapterFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;

        public GameAdapterFactory(IInteractionStateFactory interactionStateFactory, IStateControllerFactory stateControllerFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
        }

        public IGameAdapter Create(IGame game)
        {
            var gameAdapter = new GameAdapter(_interactionStateFactory, _stateControllerFactory, game);
            return gameAdapter;
        }
    }
}