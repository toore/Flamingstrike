namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectState : SelectStateBase
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        protected override IInteractionState CreateStateToEnterWhenSelected()
        {
            return _interactionStateFactory.CreateAttackState();
        }
    }
}