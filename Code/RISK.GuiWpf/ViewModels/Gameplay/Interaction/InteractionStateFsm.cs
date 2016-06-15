using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFsm
    {
        IGame Game { get; }
        IRegion SelectedRegion { get; set; }

        void Set(IInteractionState interactionState);
        bool CanClick(IRegion region);
        void OnClick(IRegion region);
    }

    public class InteractionStateFsm : IInteractionStateFsm
    {
        private IInteractionState _interactionState;
        public IGame Game { get; }
        public IRegion SelectedRegion { get; set; }

        public InteractionStateFsm(IGame game)
        {
            Game = game;
        }

        public void Set(IInteractionState interactionState)
        {
            _interactionState = interactionState;
        }

        public bool CanClick(IRegion region)
        {
            return _interactionState.CanClick(this, region);
        }

        public void OnClick(IRegion region)
        {
            _interactionState.OnClick(this, region);
        }
    }
}