using GuiWpf.ViewModels.Gameplay.Map;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainGameViewViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
    }
}