using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels
{
    public interface IGameFactory
    {
        IGame Create(ILocationSelector locationSelector);
    }
}