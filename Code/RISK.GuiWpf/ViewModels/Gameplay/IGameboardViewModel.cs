using GuiWpf.ViewModels.Gameplay.Map;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get;}
        string PlayerName { get; }
        string InformationText { get; }
    }
}