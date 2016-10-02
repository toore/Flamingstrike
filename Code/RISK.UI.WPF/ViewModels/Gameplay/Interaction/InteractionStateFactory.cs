using RISK.Core;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase);
        IInteractionState CreateSelectAttackingRegionInteractionState(ISelectAttackingRegionObserver selectAttackingRegionObserver);
        IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectAttackingRegionObserver deselectAttackingRegionObserver);
        IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        IInteractionState CreateSelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationObserver selectSourceRegionForFortificationObserver);
        IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectRegionToFortifyFromObserver deselectRegionToFortifyFromObserver);
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase)
        {
            return new DraftArmiesInteractionState(draftArmiesPhase);
        }

        public IInteractionState CreateSelectAttackingRegionInteractionState(ISelectAttackingRegionObserver selectAttackingRegionObserver)
        {
            return new SelectAttackingRegionInteractionState(selectAttackingRegionObserver);
        }

        public IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectAttackingRegionObserver deselectAttackingRegionObserver)
        {
            return new AttackInteractionState(attackPhase, selectedRegion, deselectAttackingRegionObserver);
        }

        public IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            return new SendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);
        }

        public IInteractionState CreateSelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationObserver selectSourceRegionForFortificationObserver)
        {
            return new SelectSourceRegionForFortificationInteractionState(selectSourceRegionForFortificationObserver);
        }

        public IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IDeselectRegionToFortifyFromObserver deselectRegionToFortifyFromObserver)
        {
            return new FortifyInteractionState(attackPhase, selectedRegion, deselectRegionToFortifyFromObserver);
        }
    }
}