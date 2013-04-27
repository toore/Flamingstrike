using System.Windows;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModel : ITerritoryTextViewModel
    {
        public ILocation Location { get; set; }

        public Point Position { get; set; }
        public string TerritoryName { get; set; }
        public int Armies { get; set; }
    }
}