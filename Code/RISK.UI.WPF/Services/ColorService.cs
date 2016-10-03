using System.Windows.Media;
using RISK.GameEngine.Core;

namespace RISK.UI.WPF.Services
{
    public interface IColorService
    {
        IRegionColorSettings NorthAmericaColors { get; }
        IRegionColorSettings SouthAmericaColors { get; }
        IRegionColorSettings EuropeColors { get; }
        IRegionColorSettings AfricaColors { get; }
        IRegionColorSettings AsiaColors { get; }
        IRegionColorSettings AustraliaColors { get; }

        IRegionColorSettings GetPlayerTerritoryColors(IPlayer player);

        Color SelectedTerritoryColor { get; }
    }

    public class ColorService : IColorService
    {
        public IRegionColorSettings NorthAmericaColors { get; private set; }
        public IRegionColorSettings SouthAmericaColors { get; private set; }
        public IRegionColorSettings EuropeColors { get; private set; }
        public IRegionColorSettings AfricaColors { get; private set; }
        public IRegionColorSettings AsiaColors { get; private set; }
        public IRegionColorSettings AustraliaColors { get; private set; }

        public ColorService()
        {
            NorthAmericaColors = new RegionColorSettings(
                normalStrokeColor: Colors.DarkOrange,
                normalFillColor: Colors.Yellow,
                mouseOverStrokeColor: Colors.Orange,
                mouseOverFillColor: Color.FromArgb(255, 255, 255, 210));

            SouthAmericaColors = new RegionColorSettings(
                normalStrokeColor: Colors.DarkRed,
                normalFillColor: Colors.Red,
                mouseOverStrokeColor: Color.FromArgb(255, 159, 50, 50),
                mouseOverFillColor: Color.FromArgb(255, 255, 150, 150));

            EuropeColors = new RegionColorSettings(
                normalStrokeColor: Colors.Blue,
                normalFillColor: Colors.LightBlue,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 25, 255),
                mouseOverFillColor: Color.FromArgb(255, 218, 235, 255));

            AfricaColors = new RegionColorSettings(
                normalStrokeColor: Colors.SaddleBrown,
                normalFillColor: Colors.Orange,
                mouseOverStrokeColor: Color.FromArgb(255, 169, 99, 49),
                mouseOverFillColor: Color.FromArgb(255, 255, 225, 150));

            AsiaColors = new RegionColorSettings(
                normalStrokeColor: Colors.DarkGreen,
                normalFillColor: Colors.LightGreen,
                mouseOverStrokeColor: Color.FromArgb(255, 25, 115, 25),
                mouseOverFillColor: Color.FromArgb(255, 214, 250, 214));

            AustraliaColors = new RegionColorSettings(
                normalStrokeColor: Colors.Purple,
                normalFillColor: Colors.Pink,
                mouseOverStrokeColor: Color.FromArgb(255, 156, 0, 156),
                mouseOverFillColor: Color.FromArgb(255, 255, 237, 230));
        }

        public IRegionColorSettings GetPlayerTerritoryColors(IPlayer player)
        {
            return new RegionColorSettings(
                normalStrokeColor: Colors.Black,
                normalFillColor: Colors.Gray,
                mouseOverStrokeColor: Color.FromArgb(255, 50, 50, 50),
                mouseOverFillColor: Color.FromArgb(255, 200, 200, 200));
        }

        public Color SelectedTerritoryColor
        {
            get { return Color.FromArgb(0x90, 0xEA, 0xEA, 0x15); }
        }
    }
}