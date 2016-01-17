using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class EndTurnState : IInteractionState
    {
        public bool CanClick(IStateController stateController, ITerritoryGeography selectedTerritoryGeography)
        {
            return false;
        }

        public void OnClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            throw new InvalidOperationException();
        }
    }
}