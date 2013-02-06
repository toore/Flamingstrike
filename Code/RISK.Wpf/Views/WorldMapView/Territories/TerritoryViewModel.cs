using System;
using System.Windows.Media;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public class TerritoryViewModel : IWorldMapViewModel
    {
        public string Path { get; set; }

        public Color NormalStrokeColor { get; set; }
        public Color NormalFillColor { get; set; }
        public Color MouseOverStrokeColor { get; set; }
        public Color MouseOverFillColor { get; set; }
        public Action Click { get; set; }
    }
}