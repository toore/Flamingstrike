using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifyMoveState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion selectedRegion)
        {
            return CanFortify(interactionStateFsm, selectedRegion)
                   ||
                   CanDeselect(interactionStateFsm, selectedRegion);
        }

        public void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            if (CanDeselect(interactionStateFsm, region))
            {
                Deselect(interactionStateFsm);
            }
            else if (CanFortify(interactionStateFsm, region))
            {
                Fortify(interactionStateFsm, region);
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
            var fortifySelectState = _interactionStateFactory.CreateFortifySelectState();
            interactionStateFsm.Set(fortifySelectState);

            interactionStateFsm.SelectedRegion = null;
        }

        private static bool CanFortify(IInteractionStateFsm interactionStateFsm, IRegion regionToFortify)
        {
            var canFortify = interactionStateFsm.Game.CanFortify(interactionStateFsm.SelectedRegion, regionToFortify);
            return canFortify;
        }

        private void Fortify(IInteractionStateFsm interactionStateFsm, IRegion regionToFortify)
        {
            interactionStateFsm.Game.Fortify(interactionStateFsm.SelectedRegion, regionToFortify, 1);
            var endTurnState = _interactionStateFactory.CreateEndTurnState();
            interactionStateFsm.Set(endTurnState);
        }
    }
}