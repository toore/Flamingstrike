using System.Windows;
using System.Windows.Media;

namespace RISK.WorldMap.Territories
{
    public class AlaskaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return "ALASKA"; }
        }

        public override Point NamePosition
        {
            get { return new Point(12.121866, 139.68511); }
        }

        public override string Path
        {
            get { return "m 12.121866 139.68511 41.67913 -42.115279 12.86906 -25.564934 50.513114 -8.563276 40.40058 8.563276 -45.57044 44.381753 5.76544 11.7349 -44.064802 -14.83465 z"; }
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