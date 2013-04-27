using System.Windows.Media;

namespace GuiWpf.Services
{
    public interface ITerritoryColors
    {
        Color NormalStrokeColor { get; }
        Color NormalFillColor { get; }
        Color MouseOverStrokeColor { get; }
        Color MouseOverFillColor { get; }
    }
}