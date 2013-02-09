using GuiWpf.Views.WorldMapViews;

namespace GuiWpf.Services
{
    public interface IGameEngine
    {
        WorldMapViewModel WorldMapViewModel { get; }
    }
}