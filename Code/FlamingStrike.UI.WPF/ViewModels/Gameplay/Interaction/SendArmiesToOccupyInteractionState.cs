using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class SendArmiesToOccupyInteractionState : IInteractionState
    {
        private readonly ISendArmiesToOccupyPhase _sendArmiesToOccupyPhase;

        public SendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _sendArmiesToOccupyPhase = sendArmiesToOccupyPhase;
        }

        public void OnRegionClicked(IRegion region)
        {
            _sendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(1);
        }
    }
}