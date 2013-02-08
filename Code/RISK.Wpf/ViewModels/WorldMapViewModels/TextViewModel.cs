using System.Windows;

namespace GuiWpf.ViewModels.WorldMapViewModels
{
    public class TextViewModel : IWorldMapViewModel
    {
        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}