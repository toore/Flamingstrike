using System.Windows;
using GuiWpf.ViewModels.TerritoryViewModelFactories;

namespace GuiWpf.ViewModels
{
    public class TextViewModel : IWorldMapViewModel
    {
        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}