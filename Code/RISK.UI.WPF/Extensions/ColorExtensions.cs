using System.Windows.Media;

namespace RISK.UI.WPF.Extensions
{
    public static class ColorExtensions
    {
        public static Color Darken(this Color color)
        {
            var darkerColor = Color.Multiply(color, 0.5f);
            darkerColor.A = 0xff;

            return darkerColor;
        }
    }
}