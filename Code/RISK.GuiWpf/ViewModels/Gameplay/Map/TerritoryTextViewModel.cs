using System.Windows;
using GuiWpf.TerritoryModels;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModel : ViewModelBase, ITerritoryTextViewModel
    {
        private readonly ITerritoryModel _territoryModel;

        public TerritoryTextViewModel(ITerritoryModel territoryModel)
        {
            _territoryModel = territoryModel;
        }

        public ITerritory Territory { get { return _territoryModel.Territory; } }

        public string Name { get { return _territoryModel.Name; } }
        public Point Position { get { return _territoryModel.NamePosition; } }

        private int _armies;
        public int Armies
        {
            get { return _armies; }
            set { NotifyOfPropertyChange(value, () => Armies, i => _armies = i); }
        }
    }
}