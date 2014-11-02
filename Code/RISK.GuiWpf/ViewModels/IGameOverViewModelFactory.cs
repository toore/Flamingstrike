using RISK.Application.Entities;

namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModelFactory
    {
        GameOverViewModel Create(IPlayer winner);
    }
}