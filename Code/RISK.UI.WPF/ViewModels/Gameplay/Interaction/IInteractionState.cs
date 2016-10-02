using RISK.Core;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        void OnClick(IRegion region);
    }
}