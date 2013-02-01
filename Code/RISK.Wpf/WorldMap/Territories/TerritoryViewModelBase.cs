using System.Windows;
using System.Windows.Media;

namespace RISK.WorldMap.Territories
{
    public abstract class TerritoryViewModelBase
    {
        public abstract string Name { get; }
        public abstract Point NamePosition { get; }

        public abstract string Path { get; }

        public abstract Color NormalFillColor { get; }
        public abstract Color NormalStrokeColor { get; }
        public abstract Color MouseOverFillColor { get; }
        public abstract Color MouseOverStrokeColor { get; }
    }
}