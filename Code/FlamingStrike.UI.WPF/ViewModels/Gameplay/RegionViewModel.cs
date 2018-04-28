using System;
using System.Windows.Media;
using FlamingStrike.UI.WPF.RegionModels;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IRegionViewModel : IWorldMapItemViewModel {}

    public class RegionViewModel : ViewModelBase, IRegionViewModel
    {
        private readonly IRegionModel _regionModel;
        private readonly Action<Region> _onClick;

        public RegionViewModel(IRegionModel regionModel, Action<Region> onClick)
        {
            _regionModel = regionModel;
            _onClick = onClick;
        }

        public string Path => _regionModel.Path;
        public Region Region => _regionModel.Region;

        private Color _strokeColor;
        private Color _fillColor;
        private bool _isEnabled;
        private bool _isSelected;

        public Color StrokeColor
        {
            get => _strokeColor;
            set => NotifyOfPropertyChange(value, () => StrokeColor, x => _strokeColor = x);
        }

        public Color FillColor
        {
            get => _fillColor;
            set => NotifyOfPropertyChange(value, () => FillColor, x => _fillColor = x);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => NotifyOfPropertyChange(value, () => IsEnabled, x => _isEnabled = x);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => NotifyOfPropertyChange(value, () => IsSelected, x => _isSelected = x);
        }

        public void OnClick()
        {
            _onClick(_regionModel.Region);
        }

        public void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor)
        {
            worldMapItemViewModelVisitor.Visit(this);
        }
    }
}