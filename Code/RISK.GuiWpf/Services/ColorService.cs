using System.Windows.Media;
using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public class ColorService : IColorService
    {
        public ITerritoryColors NorthAmericaColors { get; private set; }
        public ITerritoryColors SouthAmericaColors { get; private set; }
        public ITerritoryColors EuropeColors { get; private set; }
        public ITerritoryColors AfricaColors { get; private set; }
        public ITerritoryColors AsiaColors { get; private set; }
        public ITerritoryColors AustraliaColors { get; private set; }

        public ColorService()
        {
            NorthAmericaColors = new TerritoryColors(
                normalStrokeColor: Colors.DarkOrange,
                normalFillColor: Colors.Yellow,
                mouseOverStrokeColor: Colors.Orange,
                mouseOverFillColor: Color.FromArgb(255, 255, 255, 210));

            SouthAmericaColors = new TerritoryColors(
                normalStrokeColor: Colors.DarkRed,
                normalFillColor: Colors.Red,
                mouseOverStrokeColor: Color.FromArgb(255, 159, 50, 50),
                mouseOverFillColor: Color.FromArgb(255, 255, 150, 150));

            EuropeColors = new TerritoryColors(
                normalStrokeColor: Colors.Blue,
                normalFillColor: Colors.LightBlue,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 25, 255),
                mouseOverFillColor: Color.FromArgb(255, 218, 235, 255));

            AfricaColors = new TerritoryColors(
                normalStrokeColor: Colors.SaddleBrown,
                normalFillColor: Colors.Orange,
                mouseOverStrokeColor: Color.FromArgb(255, 169, 99, 49),
                mouseOverFillColor: Color.FromArgb(255, 255, 225, 150));

            AsiaColors = new TerritoryColors(
                normalStrokeColor: Colors.DarkGreen,
                normalFillColor: Colors.LightGreen,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 115, 25),
                mouseOverFillColor: Color.FromArgb(255, 214, 250, 214));

            AustraliaColors = new TerritoryColors(
                normalStrokeColor: Colors.Purple,
                normalFillColor: Colors.Pink,
                mouseOverStrokeColor: Color.FromArgb(255, 156, 0, 156),
                mouseOverFillColor: Color.FromArgb(255, 255, 237, 230));
        }

        public ITerritoryColors GetPlayerTerritoryColors(IPlayer player)
        {
            return new TerritoryColors(
                normalStrokeColor: Colors.Black,
                normalFillColor: Colors.Gray,
                mouseOverStrokeColor: Color.FromArgb(255, 50, 50, 50),
                mouseOverFillColor: Color.FromArgb(255, 200, 200, 200));
        }

        public Color SelectedTerritoryColor
        {
            get { return Color.FromArgb(0x32, 0xEA, 0xEA, 0x15); }
        }
    }
}