using System.Windows;

namespace GuiWpf.Views.WorldMap.TerritoryViewModelFactories
{
    public class TextViewModel : IWorldMapViewModel
    {
        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}