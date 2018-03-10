using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectAttackingRegionInteractionStateObserver
    {
        void Select(IRegion selectedRegion);
    }

    public class SelectAttackingRegionInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingRegionInteractionState(IAttackPhase attackPhase, ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectAttackingRegionInteractionStateObserver = selectAttackingRegionInteractionStateObserver;
        }

        public override string Title => Resources.ATTACK_SELECT_FROM_TERRITORY;

        public override bool CanEnterFortifyMode => true;

        public override bool CanEndTurn => true;

        public override void OnRegionClicked(IRegion region)
        {
            _selectAttackingRegionInteractionStateObserver.Select(region);
        }

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
        }
    }
}