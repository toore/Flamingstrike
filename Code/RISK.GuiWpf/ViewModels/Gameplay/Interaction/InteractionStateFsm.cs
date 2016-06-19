using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFsm
    {
        IRegion SelectedRegion { get; }
        void Set(IInteractionState interactionState);
        bool CanClick(IRegion region);
        void OnClick(IRegion region);
    }

    public class InteractionStateFsm : IInteractionStateFsm
    {
        private IInteractionState _interactionState;

        public IRegion SelectedRegion => _interactionState.SelectedRegion;

        public void Set(IInteractionState interactionState)
        {
            _interactionState = interactionState;
        }

        public bool CanClick(IRegion region)
        {
            return _interactionState.CanClick(region);
        }

        public void OnClick(IRegion region)
        {
            _interactionState.OnClick(region);
        }
    }
}