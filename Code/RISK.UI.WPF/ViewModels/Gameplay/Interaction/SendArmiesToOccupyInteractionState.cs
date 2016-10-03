using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class SendArmiesToOccupyInteractionState : IInteractionState
    {
        private readonly ISendArmiesToOccupyPhase _sendArmiesToOccupyPhase;

        public SendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _sendArmiesToOccupyPhase = sendArmiesToOccupyPhase;
        }

        public void OnClick(IRegion region)
        {
            _sendArmiesToOccupyPhase.SendAdditionalArmiesToOccupy(1);
        }
    }
}