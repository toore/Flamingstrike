using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectStateBase : IInteractionState
    {
        public bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            return interactionStateFsm.Game.IsCurrentPlayerOccupyingTerritory(region);
        }

        public void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            if (!CanClick(interactionStateFsm, region))
            {
                throw new InvalidOperationException();
            }

            interactionStateFsm.SelectedRegion = region;
            var stateToEnterWhenSelected = CreateStateToEnterWhenSelected();
            interactionStateFsm.Set(stateToEnterWhenSelected);
        }

        protected abstract IInteractionState CreateStateToEnterWhenSelected();
    }
}