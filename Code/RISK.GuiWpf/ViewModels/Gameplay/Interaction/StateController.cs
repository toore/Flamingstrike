using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        IGame Game { get; }
        ITerritoryId SelectedTerritoryId { get; set; }

        bool CanClick(ITerritoryId territoryId);
        void OnClick(ITerritoryId territoryId);
    }

    public class StateController : IStateController
    {
        public IInteractionState CurrentState { get; set; }
        public IGame Game { get; }
        public ITerritoryId SelectedTerritoryId { get; set; }

        public StateController(IGame game)
        {
            Game = game;
        }

        public bool CanClick(ITerritoryId territoryId)
        {
            return CurrentState.CanClick(this, territoryId);
        }

        public void OnClick(ITerritoryId territoryId)
        {
            CurrentState.OnClick(this, territoryId);
        }
    }
}