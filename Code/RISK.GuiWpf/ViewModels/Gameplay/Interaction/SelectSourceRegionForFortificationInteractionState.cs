using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface ISelectSourceRegionForFortificationObserver
    {
        void SelectSourceRegion(IRegion region);
    }

    public class SelectSourceRegionForFortificationInteractionState : IInteractionState
    {
        private readonly ISelectSourceRegionForFortificationObserver _selectSourceRegionForFortificationObserver;

        public SelectSourceRegionForFortificationInteractionState(ISelectSourceRegionForFortificationObserver selectSourceRegionForFortificationObserver)
        {
            _selectSourceRegionForFortificationObserver = selectSourceRegionForFortificationObserver;
        }

        public void OnClick(IRegion region)
        {
            _selectSourceRegionForFortificationObserver.SelectSourceRegion(region);
        }
    }
}