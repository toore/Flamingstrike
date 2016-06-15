using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion selectedRegion);
        void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region);
    }
}