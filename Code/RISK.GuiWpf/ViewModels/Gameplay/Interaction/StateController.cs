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
        private readonly IPlayer _player;

        public IInteractionState CurrentState { get; set; }

        public StateController(IInteractionStateFactory interactionStateFactory, IPlayer player, Game game)
        {
            Game = game;
            _interactionStateFactory = interactionStateFactory;
            _player = player;
        }

        public Game Game { get; private set; }

        public void SetInitialState()
        {
            CurrentState = _interactionStateFactory.CreateSelectState(this, _player);
        }
    }
}