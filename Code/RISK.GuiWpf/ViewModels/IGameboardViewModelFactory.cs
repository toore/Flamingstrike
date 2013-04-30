using GuiWpf.ViewModels.Gameplay;
using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public interface IGameboardViewModelFactory
    {
        IGameboardViewModel Create(IGame game);
    }
}