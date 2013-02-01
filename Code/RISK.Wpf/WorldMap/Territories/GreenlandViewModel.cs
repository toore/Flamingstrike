using System.Windows;
using RISK.Properties;

namespace RISK.WorldMap.Territories
{
    public class GreenlandViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.GREENLAND; }
        }
        public override Point NamePosition
        {
            get { return new Point(418.20315, 33.6191); }
        }
        public override string Path
        {
            get { return "m 418.20315 33.6191 42.42641 82.83251 104.04571 -65.659917 18.18275 -38.385797 z"; }
        }
    }
}