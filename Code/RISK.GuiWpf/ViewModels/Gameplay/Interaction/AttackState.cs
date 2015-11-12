using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class AttackState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public AttackState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, ITerritoryId territoryId)
        {
            return CanAttack(stateController, territoryId)
                   ||
                   CanDeselect(stateController, territoryId);
        }

        public void OnClick(IStateController stateController, ITerritoryId territoryId)
        {
            if (CanDeselect(stateController, territoryId))
            {
                Deselect(stateController);
            }
            else if (CanAttack(stateController, territoryId))
            {
                Attack(stateController, territoryId);
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
            stateController.CurrentState = _interactionStateFactory.CreateSelectState();
            stateController.SelectedTerritoryId = null;
        }

        private static bool CanAttack(IStateController stateController, ITerritoryId territoryId)
        {
            var attackingTerritory = stateController.SelectedTerritoryId;
            var canAttack = stateController.Game.CanAttack(attackingTerritory, territoryId);

            return canAttack;
        }

        private static void Attack(IStateController stateController, ITerritoryId attackeeTerritoryId)
        {
            var attackingTerritory = stateController.SelectedTerritoryId;
            stateController.Game.Attack(attackingTerritory, attackeeTerritoryId);
        }
    }
}