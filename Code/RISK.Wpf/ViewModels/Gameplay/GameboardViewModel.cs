using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.WorldMap;

namespace GuiWpf.ViewModels.Gameplay
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