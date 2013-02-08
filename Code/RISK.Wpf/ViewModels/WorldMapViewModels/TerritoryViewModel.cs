using System;
using System.Windows.Media;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.WorldMapViewModels
{
    public class TerritoryViewModel : ViewModelBase, ITerritoryViewModel
    {
        private readonly Action<ILocation> _clickCommand;

        public TerritoryViewModel(ILocation location, string path, Action<ILocation> clickCommand)
        {
            Location = location;
            Path = path;
            _clickCommand = clickCommand;
        }

        public ILocation Location { get; private set; }
        public string Path { get; private set; }

        private Color _normalStrokeColor;
        public Color NormalStrokeColor
        {
            get { return _normalStrokeColor; }
            set { NotifyOfPropertyChange(value, () => NormalStrokeColor, x => _normalStrokeColor = x); }
        }

        private Color _normalFillColor;
        public Color NormalFillColor
        {
            get { return _normalFillColor; }
            set { NotifyOfPropertyChange(value, () => NormalFillColor, x => _normalFillColor = x); }
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
            _clickCommand(Location);
        }
    }
}