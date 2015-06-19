using RISK.Application;
using RISK.Application.GamePlay;

namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModelFactory
    {
        GameOverViewModel Create(IPlayer winner);
    }
}