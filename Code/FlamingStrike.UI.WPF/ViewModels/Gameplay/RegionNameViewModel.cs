using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.RegionModels;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class RegionNameViewModel : ViewModelBase, IWorldMapItemViewModel
    {
        private readonly RegionModel _regionModel;

        public RegionNameViewModel(RegionModel regionModel)
        {
            _regionModel = regionModel;
        }

        public string Name => _regionModel.Name;
        public Point Position => _regionModel.NamePosition;
        public Region Region => _regionModel.Region;

        private int _armies;
        public int Armies
        {
            get => _armies;
            set => NotifyOfPropertyChange(value, () => Armies, x => _armies = x);
        }

        public void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor)
        {
            worldMapItemViewModelVisitor.Visit(this);
        }
    }
}