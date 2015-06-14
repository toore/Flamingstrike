using System.Windows;
using GuiWpf.TerritoryModels;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryTextViewModel : ViewModelBase, IWorldMapItemViewModel
    {
        private readonly ITerritoryModel _territoryModel;

        public TerritoryTextViewModel(ITerritoryModel territoryModel)
        {
            _territoryModel = territoryModel;
        }

        public string Name => _territoryModel.Name;
        public Point Position => _territoryModel.NamePosition;

        private int _armies;
        public int Armies
        {
            get { return _armies; }
            set { NotifyOfPropertyChange(value, () => Armies, x => _armies = x); }
        }

        public void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor)
        {
            worldMapItemViewModelVisitor.Visit(this);
        }

        public void UpdateArmies()
        {
            //Armies = _territoryModel.Territory.Armies;
        }
    }
}