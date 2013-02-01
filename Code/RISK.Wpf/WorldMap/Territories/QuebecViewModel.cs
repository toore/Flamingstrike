using System.Windows;
using RISK.Properties;

namespace RISK.WorldMap.Territories
{
    public class QuebecViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.QUEBEC; }
        }
        public override Point NamePosition
        {
            get { return new Point(306.5813, 163.17116); }
        }
        public override string Path
        {
            get { return "m 306.5813 163.17116 41.16371 -57.83123 56.56855 50.00255 -92.6815 42.17387 z"; }
        }
    }
}