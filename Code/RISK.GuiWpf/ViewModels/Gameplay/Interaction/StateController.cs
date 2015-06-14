using RISK.Application;
using RISK.Application.GamePlay;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        Game Game { get; }
        void SetInitialState();
    }

    public class StateController : IStateController
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IPlayerId _playerId;

        public IInteractionState CurrentState { get; set; }

        public StateController(IInteractionStateFactory interactionStateFactory, IPlayerId playerId, Game game)
        {
            Game = game;
            _interactionStateFactory = interactionStateFactory;
            _playerId = playerId;
        }

        public Game Game { get; private set; }

        public void SetInitialState()
        {
            CurrentState = _interactionStateFactory.CreateSelectState(this, _playerId);
        }
    }
}