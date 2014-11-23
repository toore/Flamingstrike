using System;
using System.Windows.Media;
using GuiWpf.TerritoryModels;
using RISK.Application.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryLayoutViewModel : ViewModelBase, ITerritoryLayoutViewModel
    {
        private readonly ITerritoryModel _territoryModel;
        private readonly Action<ITerritory> _onClick;

        public TerritoryLayoutViewModel(ITerritoryModel territoryModel, Action<ITerritory> onClick)
        {
            _territoryModel = territoryModel;
            _onClick = onClick;
        }

        public string Path { get { return _territoryModel.Path; } }

        public ITerritory Territory { get { return _territoryModel.Territory; }}

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

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { NotifyOfPropertyChange(value, () => IsSelected, x => _isSelected = x); }
        }

        public void OnClick()
        {
            _onClick(_territoryModel.Territory);
        }
    }
}