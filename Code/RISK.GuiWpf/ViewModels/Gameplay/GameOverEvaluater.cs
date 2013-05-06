using System.Linq;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameOverEvaluater : IGameOverEvaluater
    {
        public bool IsGameOver(IWorldMap worldMap)
        {
            return worldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
        }
    }
}