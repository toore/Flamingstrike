using RISK.Application.GamePlaying;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModelFactory
    {
        IGameboardViewModel Create(IGame game);
    }
}