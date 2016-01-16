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

        public bool CanClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            return CanAttack(stateController, territoryGeography)
                   ||
                   CanDeselect(stateController, territoryGeography);
        }

        public void OnClick(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            if (CanDeselect(stateController, territoryGeography))
            {
                Deselect(stateController);
            }
            else if (CanAttack(stateController, territoryGeography))
            {
                Attack(stateController, territoryGeography);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            return territoryGeography == stateController.SelectedTerritoryGeography;
        }

        private void Deselect(IStateController stateController)
        {
            stateController.CurrentState = _interactionStateFactory.CreateSelectState();
            stateController.SelectedTerritoryGeography = null;
        }

        private static bool CanAttack(IStateController stateController, ITerritoryGeography territoryGeography)
        {
            var attackingTerritory = stateController.SelectedTerritoryGeography;
            var canAttack = stateController.Game.CanAttack(attackingTerritory, territoryGeography);

            return canAttack;
        }

        private static void Attack(IStateController stateController, ITerritoryGeography attackeeTerritoryGeography)
        {
            var attackingTerritory = stateController.SelectedTerritoryGeography;
            stateController.Game.Attack(attackingTerritory, attackeeTerritoryGeography);
        }
    }
}