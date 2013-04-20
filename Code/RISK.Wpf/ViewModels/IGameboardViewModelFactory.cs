using GuiWpf.ViewModels.Gameplay;

namespace GuiWpf.ViewModels
{
    public interface IGameboardViewModelFactory
    {
        IGameboardViewModel Create();
    }
}