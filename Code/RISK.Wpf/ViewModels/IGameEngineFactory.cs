using GuiWpf.Services;

namespace GuiWpf.ViewModels
{
    public interface IGameEngineFactory
    {
        IGameEngine Create();
    }
}