using RISK.GameEngine;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectAttackingRegionObserver
    {
        void SelectRegionToAttackFrom(IRegion selectedRegion);
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