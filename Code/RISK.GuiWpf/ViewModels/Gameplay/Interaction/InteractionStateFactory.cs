namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateSelectState();
        IInteractionState CreateAttackState();
        IInteractionState CreateFortifySelectState();
        IInteractionState CreateFortifyMoveState();
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
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
    }
}