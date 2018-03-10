using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase);
        IInteractionState CreateSelectAttackingRegionInteractionState(IAttackPhase attackPhase, ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver);
        IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver);
        IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        IInteractionState CreateSelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver);
        IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver);
        IInteractionState CreateEndTurnInteractionState(IEndTurnPhase endTurnPhase);
    }

    public class InteractionStateFactory : IInteractionStateFactory
    {
        public IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase)
        {
            return new DraftArmiesInteractionState(draftArmiesPhase);
        }

        public IInteractionState CreateSelectAttackingRegionInteractionState(IAttackPhase attackPhase, ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver)
        {
            return new SelectAttackingRegionInteractionState(attackPhase, selectAttackingRegionInteractionStateObserver);
        }

        public IInteractionState CreateAttackInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            return new AttackInteractionState(attackPhase, selectedRegion, attackInteractionStateObserver);
        }

        public IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            return new SendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);
        }

        public IInteractionState CreateSelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            return new SelectSourceRegionForFortificationInteractionState(attackPhase, selectSourceRegionForFortificationInteractionStateObserver);
        }

        public IInteractionState CreateFortifyInteractionState(IAttackPhase attackPhase, IRegion selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            return new FortifyInteractionState(attackPhase, selectedRegion, fortifyInteractionStateObserver);
        }

        public IInteractionState CreateEndTurnInteractionState(IEndTurnPhase endTurnPhase)
        {
            return new EndTurnInteractionState(endTurnPhase);
        }
    }
}