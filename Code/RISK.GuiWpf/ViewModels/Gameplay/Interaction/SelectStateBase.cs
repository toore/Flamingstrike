using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectStateBase : IInteractionState
    {
        public bool CanClick(IStateController stateController, IRegion region)
        {
            var territory = stateController.Game.GetTerritory(region);

            return stateController.Game.IsCurrentPlayerOccupyingTerritory(territory);
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