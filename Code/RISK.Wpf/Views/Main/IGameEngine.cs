using GuiWpf.Views.WorldMapView;

namespace GuiWpf.Views.Main
{
    public interface IGameEngine
    {
        WorldMapViewModel GetWorldMapViewModel();
    }
}