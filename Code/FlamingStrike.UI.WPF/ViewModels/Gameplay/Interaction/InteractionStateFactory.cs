using System.Linq;
using System.Threading.Tasks;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionStateFactory
    {
        IInteractionState CreateDraftArmiesInteractionState(IDraftArmiesPhase draftArmiesPhase);
        IInteractionState CreateSelectAttackingRegionInteractionState(IAttackPhase attackPhase, ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver);
        Task<IInteractionState> CreateAttackInteractionState(IAttackPhase attackPhase, Region selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver);
        IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        IInteractionState CreateSelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver);
        Task<IInteractionState> CreateFortifyInteractionState(IAttackPhase attackPhase, Region selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver);
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

        public async Task<IInteractionState> CreateAttackInteractionState(IAttackPhase attackPhase, Region selectedRegion, IAttackInteractionStateObserver attackInteractionStateObserver)
        {
            var regionsThatCanBeAttacked = await attackPhase.GetRegionsThatCanBeAttacked(selectedRegion);

            var enabledRegions = regionsThatCanBeAttacked
                .Concat(new[] { selectedRegion }).ToList();

            return await Task.FromResult(new AttackInteractionState(attackPhase, selectedRegion, enabledRegions.ToList(), attackInteractionStateObserver));
        }

        public IInteractionState CreateSendArmiesToOccupyInteractionState(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            return new SendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);
        }

        public IInteractionState CreateSelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            return new SelectSourceRegionForFortificationInteractionState(attackPhase, selectSourceRegionForFortificationInteractionStateObserver);
        }

        public async Task<IInteractionState> CreateFortifyInteractionState(IAttackPhase attackPhase, Region selectedRegion, IFortifyInteractionStateObserver fortifyInteractionStateObserver)
        {
            var regionsThatCanBeFortified = await attackPhase.GetRegionsThatCanBeFortified(selectedRegion);

            var enabledRegions = regionsThatCanBeFortified
                .Concat(new[] { selectedRegion }).ToList();

            return await Task.FromResult(new FortifyInteractionState(attackPhase, selectedRegion, enabledRegions.ToList(), fortifyInteractionStateObserver));
        }

        public IInteractionState CreateEndTurnInteractionState(IEndTurnPhase endTurnPhase)
        {
            return new EndTurnInteractionState(endTurnPhase);
        }
    }
}