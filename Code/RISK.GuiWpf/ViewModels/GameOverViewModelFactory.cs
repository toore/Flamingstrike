using RISK.Application;
using RISK.Application.GamePlay;

namespace GuiWpf.ViewModels
{
    public class GameOverViewModelFactory : IGameOverViewModelFactory
    {
        public GameOverViewModel Create(IPlayer winner)
        {
            return new GameOverViewModel(winner);
        }
    }
}