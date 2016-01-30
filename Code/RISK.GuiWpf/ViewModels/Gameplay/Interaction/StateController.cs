using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        IGame Game { get; }
        IRegion SelectedRegion { get; set; }

        bool CanClick(IRegion region);
        void OnClick(IRegion region);
    }

    public class StateController : IStateController
    {
        public IInteractionState CurrentState { get; set; }
        public IGame Game { get; }
        public IRegion SelectedRegion { get; set; }

        public StateController(IGame game)
        {
            Game = game;
        }

        public bool CanClick(IRegion region)
        {
            return CurrentState.CanClick(this, region);
        }

        public void OnClick(IRegion region)
        {
            CurrentState.OnClick(this, region);
        }
    }
}