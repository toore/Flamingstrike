using System.Windows;

namespace GuiWpf.ViewModels.Gameboard.WorldMap
{
    public class TextViewModel : IWorldMapViewModel
    {
        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}