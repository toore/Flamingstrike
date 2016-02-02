using RISK.Application.Play.GamePhases;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesState();
        IInteractionState CreateSelectState();
        IInteractionState CreateAttackState();
        IInteractionState CreateFortifySelectState();
        IInteractionState CreateFortifyMoveState();
        IInteractionState CreateEndTurnState();
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateDraftArmiesState()
        {
            return new DraftArmiesState();
        }

        public IInteractionState CreateSelectState()
        {
            return new SelectState(this);
        }

        public IInteractionState CreateAttackState()
        {
            return new AttackState(this);
        }

        public IInteractionState CreateFortifySelectState()
        {
            return new FortifySelectState(this);
        }

        public IInteractionState CreateFortifyMoveState()
        {
            return new FortifyMoveState(this);
        }

        public IInteractionState CreateEndTurnState()
        {
            return new EndTurnState();
        }
    }
}