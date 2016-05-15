using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IStateController stateController, IRegion selectedRegion);
        void OnClick(IStateController stateController, IRegion region);
    }
}