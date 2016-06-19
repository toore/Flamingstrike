using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectInteractionState : SelectInteractionStateBase
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifySelectInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game)
            : base(interactionStateFsm, game)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        protected override IInteractionState CreateStateToEnterWhenSelected(IGame game, IRegion selectedRegion)
        {
            return _interactionStateFactory.CreateFortifyMoveInteractionState(game, selectedRegion);
        }
    }
}