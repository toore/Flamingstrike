using System;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectStateBase : IInteractionState
    {
        public bool CanClick(IStateController stateController, IRegion region)
        {
            return stateController.Game.IsCurrentPlayerOccupyingTerritory(region);
        }

        public void OnClick(IStateController stateController, IRegion region)
        {
            if (!CanClick(stateController, region))
            {
                throw new InvalidOperationException();
            }

            stateController.SelectedRegion = region;
            stateController.CurrentState = CreateStateToEnterWhenSelected();
        }

        protected abstract IInteractionState CreateStateToEnterWhenSelected();
    }
}