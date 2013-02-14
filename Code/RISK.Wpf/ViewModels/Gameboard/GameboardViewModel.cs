using GuiWpf.Services;
using GuiWpf.Views.WorldMapViews;

namespace GuiWpf.ViewModels.Gameboard
{
    public class GameboardViewModel : IGameboardViewModel
    {
        public GameboardViewModel(IGameEngine gameEngine)
        {
            WorldMapViewModel = gameEngine.WorldMapViewModel;
        }

        public WorldMapViewModel WorldMapViewModel { get; private set; }
    }
}