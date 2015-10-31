﻿using System;
using System.Windows.Media;
using GuiWpf.TerritoryModels;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class TerritoryViewModel : ViewModelBase, ITerritoryLayoutViewModel
    {
        private readonly ITerritoryModel _territoryModel;
        private readonly Action<ITerritoryId> _onClick;

        public TerritoryViewModel(ITerritoryModel territoryModel, Action<ITerritoryId> onClick)
        {
            _territoryModel = territoryModel;
            _onClick = onClick;
        }

        public string Path => _territoryModel.Path;
        public ITerritoryId TerritoryId => _territoryModel.TerritoryId;

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
            _onClick(_territoryModel.TerritoryId);
        }

        public void Accept(IWorldMapItemViewModelVisitor worldMapItemViewModelVisitor)
        {
            worldMapItemViewModelVisitor.Visit(this);
        }
    }
}