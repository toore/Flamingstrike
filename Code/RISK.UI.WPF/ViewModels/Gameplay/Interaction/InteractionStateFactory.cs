using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase);
        IInteractionState CreateSelectAttackingRegionInteractionState(ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver);
        IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver);
        IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        IInteractionState CreateSelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver);
        IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver);
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase)
        {
            return new DraftArmiesInteractionState(draftArmiesPhase);
        }

        public IInteractionState CreateSelectAttackingRegionInteractionState(ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver)
        {
            return new SelectAttackingRegionInteractionState(selectAttackingRegionInteractionStateObserver);
        }

        public IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            return new AttackInteractionState(attackPhase, selectedRegion, attackInteractionStateObserver);
        }

        public IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            return new SendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);
        }

        public IInteractionState CreateSelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            return new SelectSourceRegionForFortificationInteractionState(selectSourceRegionForFortificationInteractionStateObserver);
        }

        public IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            return new FortifyInteractionState(attackPhase, selectedRegion, fortifyInteractionStateObserver);
        }
    }
}