using RISK.Domain.GamePlaying;

namespace GuiWpf.ViewModels
{
    public interface IGameSettingStateConductor
    {
        void StartGamePlay(IGame game);
    }
}