using System.Windows;
using GuiWpf.TerritoryModels;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TitleViewModel : ViewModelBase, IWorldMapItemViewModel
    {
        private readonly ITerritoryModel _territoryModel;

        public TitleViewModel(ITerritoryModel territoryModel)
        {
            _territoryModel = territoryModel;
        }

        public string Name => _territoryModel.Name;
        public Point Position => _territoryModel.NamePosition;
        public ITerritory Territory => _territoryModel.Territory;

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
    }
}