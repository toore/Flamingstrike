using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public interface IGameStateConductor
    {
        void StartGamePlay(IGame game);
    }
}