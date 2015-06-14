using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        IPlayerId PlayerId { get; }
        ITerritory SelectedTerritory { get; }

        bool CanClick(ITerritory territory);
        void OnClick(ITerritory territory);
    }
}