using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectSourceRegionForFortificationInteractionStateObserver
    {
        void Select(IRegion region);
    }

    public class SelectSourceRegionForFortificationInteractionState : InteractionStateBase
    {
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectSourceRegionForFortificationInteractionStateObserver _selectSourceRegionForFortificationInteractionStateObserver;

        public SelectSourceRegionForFortificationInteractionState(IAttackPhase attackPhase, ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            _attackPhase = attackPhase;
            _selectSourceRegionForFortificationInteractionStateObserver = selectSourceRegionForFortificationInteractionStateObserver;
        }

        public override string Title => Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_FROM;

        public override bool CanEnterAttackMode => true;

        public override bool CanEndTurn => true;

        public override void OnRegionClicked(IRegion region)
        {
            _selectSourceRegionForFortificationInteractionStateObserver.Select(region);
        }

        public override void EndTurn()
        {
            _attackPhase.EndTurn();
        }
    }
}