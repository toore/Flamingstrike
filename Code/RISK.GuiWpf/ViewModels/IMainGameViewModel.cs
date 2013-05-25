using Caliburn.Micro;
using GuiWpf.ViewModels.Messages;

namespace GuiWpf.ViewModels
{
    public interface IMainGameViewModel : IGameSettingStateConductor, IHandle<GameSetupMessage>, IHandle<NewGameMessage>
    {
        IMainViewModel MainViewModel { get; set; }
    }
}