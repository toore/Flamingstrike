using RISK.GameEngine;

namespace RISK.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface ISelectFortificationInteractionStateObserver
    {
        void Select(IRegion region);
    }

    public class SelectFortificationInteractionState : IInteractionState
    {
        private readonly ISelectFortificationInteractionStateObserver _selectFortificationInteractionStateObserver;

        public SelectFortificationInteractionState(ISelectFortificationInteractionStateObserver selectFortificationInteractionStateObserver)
        {
            _selectFortificationInteractionStateObserver = selectFortificationInteractionStateObserver;
        }

        public void OnClick(IRegion region)
        {
            _selectFortificationInteractionStateObserver.Select(region);
        }
    }
}