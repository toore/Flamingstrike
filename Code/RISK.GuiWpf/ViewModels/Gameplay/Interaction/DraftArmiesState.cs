using System;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesState : IInteractionState
    {
        public bool CanClick(IInteractionStateFsm interactionStateFsm, IRegion selectedRegion)
        {
            return interactionStateFsm.Game.IsCurrentPlayerOccupyingTerritory(selectedRegion)
                   &&
                   interactionStateFsm.Game.GetNumberOfArmiesToDraft() > 0;
        }

        public void OnClick(IInteractionStateFsm interactionStateFsm, IRegion region)
        {
            var numberOfArmies = 1;
            interactionStateFsm.Game.PlaceDraftArmies(region, numberOfArmies);

        }
    }
}