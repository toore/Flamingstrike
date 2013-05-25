using Caliburn.Micro;

namespace GuiWpf.ViewModels
{
    public interface IMainGameViewModel : IGameStateConductor, IHandle<GameSetupMessage>, IHandle<NewGameMessage>
    {
        IMainViewModel MainViewModel { get; set; }
    }
}