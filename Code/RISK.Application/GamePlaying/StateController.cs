using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        bool PlayerShouldReceiveCardWhenTurnEnds { get; set; }
        void SetInitialState();
    }

    public class StateController : IStateController
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IPlayer _player;

        public IInteractionState CurrentState { get; set; }
        public bool PlayerShouldReceiveCardWhenTurnEnds { get; set; }

        public StateController(IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            _interactionStateFactory = interactionStateFactory;
            _player = player;
        }

        public void SetInitialState()
        {
            CurrentState = _interactionStateFactory.CreateSelectState(this, _player);
        }
    }
}