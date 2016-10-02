using System.Windows.Media;

namespace RISK.UI.WPF.Services
{
    public interface IRegionColorSettings
    {
        Color NormalStrokeColor { get; }
        Color NormalFillColor { get; }
        Color MouseOverStrokeColor { get; }
        Color MouseOverFillColor { get; }
    }

    public class RegionColorSettings : IRegionColorSettings
    {
        public RegionColorSettings(Color normalStrokeColor, Color normalFillColor, Color mouseOverStrokeColor, Color mouseOverFillColor)
        {
            MouseOverFillColor = mouseOverFillColor;
            MouseOverStrokeColor = mouseOverStrokeColor;
            NormalFillColor = normalFillColor;
            NormalStrokeColor = normalStrokeColor;
        }

        public Color NormalStrokeColor { get; private set; }
        public Color NormalFillColor { get; private set; }
        public Color MouseOverStrokeColor { get; private set; }
        public Color MouseOverFillColor { get; private set; }
    }
}