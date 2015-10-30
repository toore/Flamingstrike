using RISK.Application;

namespace GuiWpf.ViewModels
{
    public class GameOverViewModelFactory : IGameOverViewModelFactory
    {
        public GameOverViewModel Create(IPlayerId winner)
        {
            return new GameOverViewModel(winner);
        }
    }
}