namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectState : SelectStateBase
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifySelectState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        protected override IInteractionState CreateStateToEnterWhenSelected()
        {
            return _interactionStateFactory.CreateFortifyMoveState();
        }
    }
}