using System.Windows;

namespace GuiWpf.ViewModels.Gameplay.WorldMap
{
    public class TextViewModel : IWorldMapViewModel
    {
        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}