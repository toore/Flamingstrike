using GuiWpf.ViewModels.Gameplay.WorldMap;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainGameViewViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
    }
}