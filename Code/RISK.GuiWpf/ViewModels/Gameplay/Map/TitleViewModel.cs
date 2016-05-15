using System.Windows;
using GuiWpf.TerritoryModels;
using RISK.Core;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TitleViewModel : ViewModelBase, IWorldMapItemViewModel
    {
        private readonly IRegionModel _regionModel;

        public TitleViewModel(IRegionModel regionModel)
        {
            _regionModel = regionModel;
        }

        public string Name => _regionModel.Name;
        public Point Position => _regionModel.NamePosition;
        public IRegion Region => _regionModel.Region;

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