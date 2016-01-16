using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        bool CanClick(IStateController stateController, ITerritoryGeography territoryGeography);
        void OnClick(IStateController stateController,ITerritoryGeography territoryGeography);
    }
}