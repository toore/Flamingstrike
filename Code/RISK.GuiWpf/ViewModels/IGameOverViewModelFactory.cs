using RISK.Domain.Entities;

namespace GuiWpf.ViewModels
{
    public interface IGameOverViewModelFactory
    {
        GameOverViewModel Create(IPlayer winner);
    }
}