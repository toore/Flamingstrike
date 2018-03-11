using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesInteractionState : InteractionStateBase
    {
        private readonly IDraftArmiesPhase _draftArmiesPhase;

        public DraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase)
        {
            _draftArmiesPhase = draftArmiesPhase;
        }

        public override string Title => string.Format(Resources.DRAFT_ARMIES, _draftArmiesPhase.NumberOfArmiesToDraft);

        public override IReadOnlyList<IRegion> EnabledRegions => _draftArmiesPhase.RegionsAllowedToDraftArmies;

        public override void OnRegionClicked(IRegion region)
        {
            const int numberOfArmies = 1;
            _draftArmiesPhase.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}