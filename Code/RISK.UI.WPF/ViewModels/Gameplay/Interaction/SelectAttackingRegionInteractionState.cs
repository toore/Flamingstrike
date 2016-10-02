using RISK.Core;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectAttackingRegionObserver
    {
        void SelectRegionToAttackFrom(IRegion region);
    }

    public class SelectAttackingRegionInteractionState : IInteractionState
    {
        private readonly ISelectAttackingRegionObserver _selectAttackingRegionObserver;

        public SelectAttackingRegionInteractionState(ISelectAttackingRegionObserver selectAttackingRegionObserver)
        {
            _selectAttackingRegionObserver = selectAttackingRegionObserver;
        }

        public void OnClick(IRegion region)
        {
            _selectAttackingRegionObserver.SelectRegionToAttackFrom(region);
        }
    }
}