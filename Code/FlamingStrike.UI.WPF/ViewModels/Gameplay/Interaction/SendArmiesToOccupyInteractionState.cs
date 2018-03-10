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

        public override void OnRegionClicked(IRegion region)
        {
            _sendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(1);
        }
    }
}