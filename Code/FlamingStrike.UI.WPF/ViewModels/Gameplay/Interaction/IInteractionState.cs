using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        void OnClick(IRegion region);
    }
}