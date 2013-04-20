using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public interface IGameFactory
    {
        IGame Create();
    }
}