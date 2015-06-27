using System;
using System.Linq;
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

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            return CanAttack(stateController, territory)
                   ||
                   CanDeselect(stateController, territory);
        }

        public void OnClick(IStateController stateController, ITerritory territory)
        {
            if (CanDeselect(stateController, territory))
            {
                Deselect(stateController, territory);
            }
            else if (CanAttack(stateController, territory))
            {
                Attack(stateController, territory);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanAttack(IStateController stateController, ITerritory territory)
        {
            var attackingTerritory = stateController.SelectedTerritory;
            var canAttack = stateController.Game.GetAttackeeCandidates(attackingTerritory).Contains(territory);

            return canAttack;
        }

        private static bool CanDeselect(IStateController stateController, ITerritory territory)
        {
            return territory == stateController.SelectedTerritory;
        }

        private static void Attack(IStateController stateController, ITerritory attackeeTerritory)
        {
            var attackingTerritory = stateController.SelectedTerritory;
            var attackResult = stateController.Game.Attack(attackingTerritory, attackeeTerritory);

            // fire event for gameboard territories change
        }

        private void Deselect(IStateController stateController, ITerritory territory)
        {
            //if (CanSelect(location))
            {
                stateController.CurrentState = _interactionStateFactory.CreateSelectState();
                stateController.SelectedTerritory = null;
            }
        }
    }
}