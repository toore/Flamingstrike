using GuiWpf.Services;
using GuiWpf.ViewModels.Gameboard.WorldMap;

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