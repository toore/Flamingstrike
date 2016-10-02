using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        void OnClick(IRegion region);
    }
}