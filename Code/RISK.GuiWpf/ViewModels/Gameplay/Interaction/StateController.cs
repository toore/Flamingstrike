using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        IGame Game { get; }
        ITerritoryGeography SelectedTerritoryGeography { get; set; }

        bool CanClick(ITerritoryGeography territoryGeography);
        void OnClick(ITerritoryGeography territoryGeography);
    }

    public class StateController : IStateController
    {
        public IInteractionState CurrentState { get; set; }
        public IGame Game { get; }
        public ITerritoryGeography SelectedTerritoryGeography { get; set; }

        public StateController(IGame game)
        {
            Game = game;
        }

        public bool CanClick(ITerritoryGeography territoryGeography)
        {
            return CurrentState.CanClick(this, territoryGeography);
        }

        public void OnClick(ITerritoryGeography territoryGeography)
        {
            CurrentState.OnClick(this, territoryGeography);
        }
    }
}