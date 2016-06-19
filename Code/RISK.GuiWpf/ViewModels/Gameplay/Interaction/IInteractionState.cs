using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        IRegion SelectedRegion { get; }
        bool CanClick(IRegion region);
        void OnClick(IRegion region);
    }
}