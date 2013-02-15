using GuiWpf.ViewModels.Gameboard.WorldMap;
using GuiWpf.Views.WorldMapViews;

namespace GuiWpf.ViewModels.Gameboard
{
    public interface IGameboardViewModel : IMainGameViewViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
    }
}