using System;
using RISK.Application.Play;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IStateController
    {
        IInteractionState CurrentState { get; set; }
        IGame Game { get; }
        ITerritory SelectedTerritory { get; set; }

        bool CanClick(ITerritory territory);
        void OnClick(ITerritory territory);
    }

    public class StateController : IStateController
    {
        public IInteractionState CurrentState { get; set; }
        public IGame Game { get; }
        public ITerritory SelectedTerritory { get; set; }

        public StateController(IGame game)
        {
            Game = game;
        }

        public bool CanClick(ITerritory territory)
        {
            return CurrentState.CanClick(this, territory);
        }

        public void OnClick(ITerritory territory)
        {
            CurrentState.OnClick(this, territory);
        }
    }
}