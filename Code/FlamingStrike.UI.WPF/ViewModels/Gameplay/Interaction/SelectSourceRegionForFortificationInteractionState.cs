using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectSourceRegionForFortificationInteractionStateObserver
    {
        void Select(IRegion region);
    }

    public class SelectSourceRegionForFortificationInteractionState : IInteractionState
    {
        private readonly ISelectSourceRegionForFortificationInteractionStateObserver _selectSourceRegionForFortificationInteractionStateObserver;

        public SelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationInteractionStateObserver selectSourceRegionForFortificationInteractionStateObserver)
        {
            _selectSourceRegionForFortificationInteractionStateObserver = selectSourceRegionForFortificationInteractionStateObserver;
        }

        public void OnClick(IRegion region)
        {
            _selectSourceRegionForFortificationInteractionStateObserver.Select(region);
        }
    }
}