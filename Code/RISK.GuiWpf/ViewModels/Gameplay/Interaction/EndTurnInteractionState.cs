using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class EndTurnInteractionState : IInteractionState
    {
        public IRegion SelectedRegion => null;

        public bool CanClick(IRegion region)
        {
            return false;
        }

        public void OnClick(IRegion region)
        {
            throw new InvalidOperationException();
        }
    }
}