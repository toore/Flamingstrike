using System.Windows;
using System.Windows.Media;

namespace RISK.WorldMap.Territories
{
    public abstract class TerritoryViewModelBase
    {
        public abstract string Name { get; }
        public abstract Point NamePosition { get; }

        public abstract string Path { get; }

        public Color NormalStrokeColor { get; set; }
        public Color NormalFillColor { get; set; }
        public Color MouseOverStrokeColor { get; set; }
        public Color MouseOverFillColor { get; set; }
    }
}