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
                mouseOverFillColor: Colors.LightYellow);

            SouthAmericaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkRed,
                normalFillColor: Colors.Red,
                mouseOverStrokeColor: Colors.Brown,
                mouseOverFillColor: Colors.PeachPuff);

            EuropeColors = new ContinentColors(
                normalStrokeColor: Colors.Blue,
                normalFillColor: Colors.LightBlue,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 25, 255),
                mouseOverFillColor: Colors.AliceBlue);

            AfricaColors = new ContinentColors(
                normalStrokeColor: Colors.SaddleBrown,
                normalFillColor: Colors.Orange,
                mouseOverStrokeColor: Colors.Sienna,
                mouseOverFillColor: Colors.LightYellow);

            AsiaColors = new ContinentColors(
                normalStrokeColor: Colors.DarkGreen,
                normalFillColor: Colors.LightGreen,
                mouseOverStrokeColor: Colors.Green,
                mouseOverFillColor: Colors.PaleGreen);

            AustraliaColors = new ContinentColors(
                normalStrokeColor: Colors.Purple,
                normalFillColor: Colors.Pink,
                mouseOverStrokeColor: Color.FromArgb(255, 156, 0, 156),
                mouseOverFillColor: Color.FromArgb(255, 255, 232, 243));
        }
    }
}