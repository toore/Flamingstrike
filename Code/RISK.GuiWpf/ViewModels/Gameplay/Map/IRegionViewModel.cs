﻿using System.Windows.Media;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public interface IRegionViewModel : IWorldMapItemViewModel
    {
        Color StrokeColor { get; set; }
        Color FillColor { get; set; }
        Color MouseOverStrokeColor { get; set; }
        Color MouseOverFillColor { get; set; }

        void OnClick();
    }
}