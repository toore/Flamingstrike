using GuiWpf.ViewModels.Gameplay.Map;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
        void EndTurn();
    }
}