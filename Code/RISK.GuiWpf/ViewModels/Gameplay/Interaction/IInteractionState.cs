using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }

        bool CanClick(ITerritory territory);
        void OnClick(ITerritory territory);
    }
}