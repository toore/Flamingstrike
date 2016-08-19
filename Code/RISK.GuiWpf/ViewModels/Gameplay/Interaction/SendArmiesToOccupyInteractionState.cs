using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SendArmiesToOccupyInteractionState : IInteractionState
    {
        private readonly IInteractionContext _interactionContext;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly IRegion _conqueredRegion;

        public SendArmiesToOccupyInteractionState(IInteractionContext interactionContext, IInteractionStateFactory interactionStateFactory, IGame game, IRegion selectedRegion, IRegion conqueredRegion)
        {
            SelectedRegion = selectedRegion;
            _interactionContext = interactionContext;
            _interactionStateFactory = interactionStateFactory;
            _game = game;
            _conqueredRegion = conqueredRegion;
        }

        public IRegion SelectedRegion { get; }

        public bool CanClick(IRegion region)
        {
            return region == _conqueredRegion;
        }

        public void OnClick(IRegion region)
        {
            _game.SendArmiesToOccupy(1);

            EnterAttackState();
        }

        private void EnterAttackState()
        {
            var attackInteractionState = _interactionStateFactory.CreateAttackInteractionState(_game, _conqueredRegion);
            _interactionContext.Set(attackInteractionState);
        }
    }
}