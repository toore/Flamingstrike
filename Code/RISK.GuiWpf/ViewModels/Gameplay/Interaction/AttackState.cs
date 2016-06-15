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

        public bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion selectedRegion)
        {
            return CanAttack(interactionStateFsm, selectedRegion)
                   ||
                   CanDeselect(interactionStateFsm, selectedRegion);
        }

        public void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            if (CanDeselect(interactionStateFsm, region))
            {
                Deselect(interactionStateFsm);
            }
            else if (CanAttack(interactionStateFsm, region))
            {
                Attack(interactionStateFsm, region);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static bool CanDeselect(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            return region == interactionStateFsm.SelectedRegion;
        }

        private void Deselect(IInteractionStateFsm interactionStateFsm)
        {
            var selectState = _interactionStateFactory.CreateSelectState();
            interactionStateFsm.Set(selectState);

            interactionStateFsm.SelectedRegion = null;
        }

        private static bool CanAttack(IInteractionStateFsm interactionStateFsm, IRegion attackeeRegion)
        {
            var canAttack = interactionStateFsm.Game.CanAttack(interactionStateFsm.SelectedRegion, attackeeRegion);

            return canAttack;
        }

        private static void Attack(IInteractionStateFsm interactionStateFsm, IRegion attackedRegion)
        {
            interactionStateFsm.Game.Attack(interactionStateFsm.SelectedRegion, attackedRegion);
        }
    }
}