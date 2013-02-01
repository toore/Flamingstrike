using System.Windows;
using RISK.Properties;

namespace RISK.WorldMap.Territories
{
    public class WesternUnitedStatesViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.WESTERN_UNITED_STATES; }
        }
        public override Point NamePosition
        {
            get { return new Point(126.26907, 176.05061); }
        }
        public override string Path
        {
            get { return "m 126.26907 176.05061 86.87312 0.25254 7.57614 4.54568 -8.83883 61.87185 -63.38708 27.77919 -59.851534 -22.47589 z"; }
        }
    }
}