using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectAttackingRegionInteractionStateObserver
    {
        void Select(IRegion selectedRegion);
    }

    public class SelectAttackingRegionInteractionState : IInteractionState
    {
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingRegionInteractionState(ISelectAttackingRegionInteractionStateObserver selectAttackingRegionInteractionStateObserver)
        {
            _selectAttackingRegionInteractionStateObserver = selectAttackingRegionInteractionStateObserver;
        }

        public void OnClick(IRegion region)
        {
            _selectAttackingRegionInteractionStateObserver.Select(region);
        }
    }
}