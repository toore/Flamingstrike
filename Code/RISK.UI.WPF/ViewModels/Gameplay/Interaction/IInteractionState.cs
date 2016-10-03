using RISK.GameEngine;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        void OnClick(IRegion region);
    }
}