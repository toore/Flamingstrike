using System.Windows;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModel : ViewModelBase, ITerritoryTextViewModel
    {
        public ITerritory Territory { get; set; }

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