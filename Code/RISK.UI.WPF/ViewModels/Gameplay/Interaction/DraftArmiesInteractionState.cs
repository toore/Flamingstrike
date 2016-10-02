using RISK.Core;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesInteractionState : IInteractionState
    {
        private readonly IDraftArmiesPhase _draftArmiesPhase;

        public DraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase)
        {
            _draftArmiesPhase = draftArmiesPhase;
        }

        public void OnClick(IRegion region)
        {
            const int numberOfArmies = 1;
            _draftArmiesPhase.PlaceDraftArmies(region, numberOfArmies);
        }
    }
}