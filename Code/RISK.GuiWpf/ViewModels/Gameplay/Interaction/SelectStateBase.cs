using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectStateBase : IInteractionState
    {
        public bool CanClick(IStateController stateController, ITerritoryId territoryId)
        {
            return stateController.Game.IsCurrentPlayerOccupyingTerritory(territoryId);
        }

        public void OnClick(IStateController stateController, ITerritoryId territoryId)
        {
            if (!CanClick(stateController, territoryId))
            {
                throw new InvalidOperationException();
            }

            stateController.SelectedTerritoryId = territoryId;
            stateController.CurrentState = CreateStateToEnterWhenSelected();
        }

        protected abstract IInteractionState CreateStateToEnterWhenSelected();
    }
}