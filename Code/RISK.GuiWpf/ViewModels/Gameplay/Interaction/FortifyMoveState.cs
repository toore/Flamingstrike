using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifyMoveState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, ITerritoryId territoryId)
        {
            return CanFortify(stateController, territoryId)
                   ||
                   CanDeselect(stateController, territoryId);
        }

        public void OnClick(IStateController stateController, ITerritoryId territoryId)
        {
            if (CanDeselect(stateController, territoryId))
            {
                Deselect(stateController);
            }
            else if (CanFortify(stateController, territoryId))
            {
                Fortify(stateController, territoryId);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IStateController stateController, ITerritoryId territoryId)
        {
            return territoryId == stateController.SelectedTerritoryId;
        }

        private void Deselect(IStateController stateController)
        {
            stateController.CurrentState = _interactionStateFactory.CreateFortifySelectState();
            stateController.SelectedTerritoryId = null;
        }

        private void Fortify(IStateController stateController, ITerritoryId territoryId)
        {
            stateController.Game.Fortify(stateController.SelectedTerritoryId, territoryId);
            stateController.CurrentState = _interactionStateFactory.CreateEndTurnState();
        }

        private static bool CanFortify(IStateController stateController, ITerritoryId territoryIdToFortify)
        {
            var sourceTerritory = stateController.SelectedTerritoryId;
            var canFortify = stateController.Game.CanFortify(sourceTerritory, territoryIdToFortify);
            return canFortify;
        }
    }
}