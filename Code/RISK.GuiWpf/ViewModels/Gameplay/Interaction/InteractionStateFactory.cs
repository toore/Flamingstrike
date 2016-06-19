using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IGame game);
        IInteractionState CreateSelectInteractionState(IGame game);
        IInteractionState CreateAttackInteractionState(IGame game, IRegion selectedRegion);
        IInteractionState CreateFortifySelectInteractionState(IGame game);
        IInteractionState CreateFortifyMoveInteractionState(IGame game, IRegion selectedRegion);
        IInteractionState CreateEndTurnInteractionState();
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        private readonly IInteractionStateFsm _interactionStateFsm;

        public InteractionStateFactory(IInteractionStateFsm interactionStateFsm)
        {
            _interactionStateFsm = interactionStateFsm;
        }

        public IInteractionState CreateDraftArmiesInteractionState(IGame game)
        {
            return new DraftArmiesInteractionState(_interactionStateFsm, this, game);
        }

        public IInteractionState CreateSelectInteractionState(IGame game)
        {
            return new SelectInteractionState(_interactionStateFsm, this, game);
        }

        public IInteractionState CreateAttackInteractionState(IGame game, IRegion selectedRegion)
        {
            return new AttackInteractionState(_interactionStateFsm, this, game, selectedRegion);
        }

        public IInteractionState CreateFortifySelectInteractionState(IGame game)
        {
            return new FortifySelectInteractionState(_interactionStateFsm, this, game);
        }

        public IInteractionState CreateFortifyMoveInteractionState(IGame game, IRegion selectedRegion)
        {
            return new FortifyMoveInteractionState(_interactionStateFsm, this, game, selectedRegion);
        }

        public IInteractionState CreateEndTurnInteractionState()
        {
            return new EndTurnInteractionState();
        }
    }
}