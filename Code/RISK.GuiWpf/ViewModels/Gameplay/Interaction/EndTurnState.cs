using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class EndTurnState : IInteractionState
    {
        public bool CanClick(IStateController stateController, ITerritoryId territoryId)
        {
            throw new NotImplementedException();
        }

        public void OnClick(IStateController stateController, ITerritoryId territoryId)
        {
            throw new NotImplementedException();
        }
    }
}