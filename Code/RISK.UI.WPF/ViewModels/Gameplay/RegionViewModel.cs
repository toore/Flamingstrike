using System;
using System.Windows.Media;
using RISK.GameEngine;
using RISK.UI.WPF.RegionModels;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IRegionViewModel : IWorldMapItemViewModel
    {
        Color StrokeColor { get; set; }
        Color FillColor { get; set; }
        Color MouseOverStrokeColor { get; set; }
        Color MouseOverFillColor { get; set; }

        void OnClick();
    }

    public class RegionViewModel : ViewModelBase, IRegionViewModel
    {
        private readonly IRegionModel _regionModel;
        private readonly Action<IRegion> _onClick;

        public RegionViewModel(IRegionModel regionModel, Action<IRegion> onClick)
        {
            _regionModel = regionModel;
            _onClick = onClick;
        }

        public string Path => _regionModel.Path;
        public IRegion Region => _regionModel.Region;

        private Color _strokeColor;
        public Color StrokeColor
        {
            get { return _strokeColor; }
            set { NotifyOfPropertyChange(value, () => StrokeColor, x => _strokeColor = x); }
        }

        private Color _fillColor;
        public Color FillColor
        {
            get { return _fillColor; }
            set { NotifyOfPropertyChange(value, () => FillColor, x => _fillColor = x); }
        }

        private Color _mouseOverStrokeColor;
        public Color MouseOverStrokeColor
        {
            get { return _mouseOverStrokeColor; }
            set { NotifyOfPropertyChange(value, () => MouseOverStrokeColor, x => _mouseOverStrokeColor = x); }
        }

        private Color _mouseOverFillColor;
        public Color MouseOverFillColor
        {
            get { return _mouseOverFillColor; }
            set { NotifyOfPropertyChange(value, () => MouseOverFillColor, x => _mouseOverFillColor = x); }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { NotifyOfPropertyChange(value, () => IsEnabled, x => _isEnabled = x); }
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