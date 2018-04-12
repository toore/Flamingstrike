using System.Collections.Generic;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class SendArmiesToOccupyInteractionState : InteractionStateBase
    {
        private readonly ISendArmiesToOccupyPhase _sendArmiesToOccupyPhase;

        public SendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _sendArmiesToOccupyPhase = sendArmiesToOccupyPhase;
        }

        public override string Title => Resources.SEND_ARMIES_TO_OCCUPY;

        public override IReadOnlyList<Region> EnabledRegions => new[] { _sendArmiesToOccupyPhase.OccupiedRegion };

        public override Maybe<Region> SelectedRegion => Maybe<Region>.Create(_sendArmiesToOccupyPhase.AttackingRegion);

        public override bool CanUserSelectNumberOfArmies => true;

        public override void OnRegionClicked(Region region)
        {
            _sendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(1);
        }
    }
}