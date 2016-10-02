﻿using System.Windows;
using RISK.Core;
using RISK.UI.WPF.RegionModels;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public class RegionNameViewModel : ViewModelBase, IWorldMapItemViewModel
    {
        private readonly IRegionModel _regionModel;

        public RegionNameViewModel(IRegionModel regionModel)
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