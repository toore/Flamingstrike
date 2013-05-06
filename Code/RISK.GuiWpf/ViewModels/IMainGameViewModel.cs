using Caliburn.Micro;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.ViewModels
{
    public interface IMainGameViewModel : IGameStateConductor, IHandle<GameSetupMessage>
    {
        IMainViewModel MainViewModel { get; set; }
    }
}