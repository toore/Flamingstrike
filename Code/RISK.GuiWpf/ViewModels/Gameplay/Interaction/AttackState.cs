using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class AttackState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public AttackState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, IRegion selectedRegion)
        {
            return CanAttack(stateController, selectedRegion)
                   ||
                   CanDeselect(stateController, selectedRegion);
        }

        public void OnClick(IStateController stateController, IRegion region)
        {
            if (CanDeselect(stateController, region))
            {
                Deselect(stateController);
            }
            else if (CanAttack(stateController, region))
            {
                Attack(stateController, region);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IStateController stateController, IRegion region)
        {
            return region == stateController.SelectedRegion;
        }

        private void Deselect(IStateController stateController)
        {
            stateController.CurrentState = _interactionStateFactory.CreateSelectState();
            stateController.SelectedRegion = null;
        }

        private static bool CanAttack(IStateController stateController, IRegion attackeeRegion)
        {
            var canAttack = stateController.Game.CanAttack(stateController.SelectedRegion, attackeeRegion);

            return canAttack;
        }

        private static void Attack(IStateController stateController, IRegion attackedRegion)
        {
            stateController.Game.Attack(stateController.SelectedRegion, attackedRegion);
        }
    }
}