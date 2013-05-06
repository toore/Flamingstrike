using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameOverEvaluater
    {
        bool IsGameOver(IWorldMap worldMap);
    }
}