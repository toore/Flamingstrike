using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class EndTurnState : IInteractionState
    {
        public bool CanClick(IStateController stateController, IRegion selectedRegion)
        {
            return false;
        }

        public void OnClick(IStateController stateController, IRegion region)
        {
            throw new InvalidOperationException();
        }
    }
}