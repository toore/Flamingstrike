using System.Windows.Media;

namespace GuiWpf.Services
{
    public class RegionColorSetting : ITerritoryColors
    {
        public RegionColorSetting(Color normalStrokeColor, Color normalFillColor, Color mouseOverStrokeColor, Color mouseOverFillColor)
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