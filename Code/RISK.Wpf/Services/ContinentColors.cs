using System.Windows.Media;

namespace GuiWpf.Services
{
    public class ContinentColors
    {
        public ContinentColors(Color normalStrokeColor, Color normalFillColor, Color mouseOverStrokeColor, Color mouseOverFillColor)
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