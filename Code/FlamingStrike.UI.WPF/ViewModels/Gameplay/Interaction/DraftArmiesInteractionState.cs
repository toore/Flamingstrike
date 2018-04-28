using System.Collections.Generic;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

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

        public override IReadOnlyList<Region> EnabledRegions => _draftArmiesPhase.RegionsAllowedToDraftArmies;

        public override bool CanUserSelectNumberOfArmies => true;

        public override void OnRegionClicked(Region region)
        {
            const int numberOfArmies = 1;
            _draftArmiesPhase.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}