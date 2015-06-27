using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IStateController stateController, ITerritory territory);
        void OnClick(IStateController stateController,ITerritory territory);
    }
}