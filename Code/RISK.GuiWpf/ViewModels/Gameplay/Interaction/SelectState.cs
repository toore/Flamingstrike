using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

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
            stateController.CurrentState = _interactionStateFactory.CreateAttackState();
        }
    }
}