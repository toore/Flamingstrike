using System.Windows;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModel : ViewModelBase, ITerritoryTextViewModel
    {
        public ILocation Location { get; set; }

        public Point Position { get; set; }
        public string TerritoryName { get; set; }

        private int _armies;
        public int Armies
        {
            get { return _armies; }
            set { NotifyOfPropertyChange(value, () => Armies, i => _armies = i); }
        }
    }
}