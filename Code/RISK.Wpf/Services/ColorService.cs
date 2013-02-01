using System.Windows.Media;

namespace RISK.Services
{
    public class ColorService : IColorService
    {
        private readonly ContinentColors _northAmericaColors;

        public ColorService()
        {
            _northAmericaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkOrange,
                normalFillColor: Colors.Yellow,
                mouseOverStrokeColor: Colors.Orange,
                mouseOverFillColor: Colors.LightYellow);
        }

        public ContinentColors GetNorthAmericaColors()
        {
            return _northAmericaColors;
        }
    }
}