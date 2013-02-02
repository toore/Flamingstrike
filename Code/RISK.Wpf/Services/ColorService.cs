using System.Windows.Media;

namespace GuiWpf.Services
{
    public class ColorService : IColorService
    {
        public ContinentColors NorthAmericaColors { get; private set; }
        public ContinentColors SouthAmericaColors { get; private set; }
        public ContinentColors EuropeColors { get; private set; }
        public ContinentColors AfricaColors { get; private set; }
        public ContinentColors AsiaColors { get; private set; }
        public ContinentColors AustraliaColors { get; private set; }

        public ColorService()
        {
            NorthAmericaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkOrange,
                normalFillColor: Colors.Yellow,
                mouseOverStrokeColor: Colors.Orange,
                mouseOverFillColor: Color.FromArgb(255, 255, 255, 210));

            SouthAmericaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkRed,
                normalFillColor: Colors.Red,
                mouseOverStrokeColor: Color.FromArgb(255, 159, 50, 50),
                mouseOverFillColor: Color.FromArgb(255, 255, 150, 150));

            EuropeColors = new ContinentColors(
                normalStrokeColor: Colors.Blue,
                normalFillColor: Colors.LightBlue,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 25, 255),
                mouseOverFillColor: Color.FromArgb(255, 218, 235, 255));

            AfricaColors = new ContinentColors(
                normalStrokeColor: Colors.SaddleBrown,
                normalFillColor: Colors.Orange,
                mouseOverStrokeColor: Color.FromArgb(255, 169, 99, 49),
                mouseOverFillColor: Color.FromArgb(255, 255, 225, 150));

            AsiaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkGreen,
                normalFillColor: Colors.LightGreen,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 115, 25),
                mouseOverFillColor: Color.FromArgb(255, 214, 250, 214));

            AustraliaColors = new ContinentColors(
                normalStrokeColor: Colors.Purple,
                normalFillColor: Colors.Pink,
                mouseOverStrokeColor: Color.FromArgb(255, 156, 0, 156),
                mouseOverFillColor: Color.FromArgb(255, 255, 237, 230));
        }
    }
}