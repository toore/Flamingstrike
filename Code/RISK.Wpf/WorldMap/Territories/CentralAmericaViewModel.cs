using System.Windows;
using System.Windows.Media;

namespace RISK.WorldMap.Territories
{
    public class CentralAmericaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return "CENTRAL AMERICA"; }
        }

        public override Point NamePosition
        {
            get { return new Point(97.984797, 251.81205); }
        }

        public override string Path
        {
            get { return "m 97.984797 251.81205 39.395953 84.0952 120.46069 67.93276 6.81853 -7.32361 -43.6891 -69.44799 -43.6891 2.77792 4.04061 -29.04188 -33.08249 -30.55712 z"; }
        }

        public override Color NormalFillColor
        {
            get { return Colors.DarkOrange; }
        }

        public override Color NormalStrokeColor
        {
            get { return Colors.Yellow; }
        }

        public override Color MouseOverFillColor
        {
            get { return Colors.OrangeRed; }
        }

        public override Color MouseOverStrokeColor
        {
            get { return Colors.White; }
        }
    }
}