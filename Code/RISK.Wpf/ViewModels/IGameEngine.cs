using GuiWpf.Views.WorldMap;

namespace GuiWpf.ViewModels
{
    public interface IGameEngine
    {
        WorldMapViewModel WorldMapViewModel { get; }
    }
}