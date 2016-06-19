using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectInteractionState : SelectInteractionStateBase
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game)
            : base(interactionStateFsm, game)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        protected override IInteractionState CreateStateToEnterWhenSelected(IGame game, IRegion selectedRegion)
        {
            return _interactionStateFactory.CreateAttackInteractionState(game, selectedRegion);
        }
    }
}