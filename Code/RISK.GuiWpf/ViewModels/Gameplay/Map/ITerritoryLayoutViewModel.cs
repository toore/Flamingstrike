﻿using System.Windows.Media;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface ITerritoryLayoutViewModel : IWorldMapViewModel
    {
        ILocation Location { get; }
        
        Color NormalStrokeColor { get; set; }
        Color NormalFillColor { get; set; }
        Color MouseOverStrokeColor { get; set; }
        Color MouseOverFillColor { get; set; }

        bool IsEnabled { get; set; }
        bool IsSelected { get; set; }

        void OnClick();
    }
}