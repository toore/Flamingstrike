using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class EndTurnState : IInteractionState
    {
        public bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion selectedRegion)
        {
            return false;
        }

        public void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            throw new InvalidOperationException();
        }
    }
}