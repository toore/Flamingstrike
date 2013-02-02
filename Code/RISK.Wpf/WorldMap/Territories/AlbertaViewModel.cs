using System.Collections.Generic;
using System.Windows;
using RISK.Properties;

namespace RISK.WorldMap.Territories
{
    public class AlbertaViewModel : TerritoryViewModelBase
    {
        public override string Name
        {
            get { return Resources.ALBERTA; }
        }
        public override Point NamePosition
        {
            get { return new Point(80.76399, 175.79807); }
        }
        public override string Path
        {
            get { return "m 125.76399 175.79807 87.12566 0.25254 31.8198 -57.07362 -132.07745 -2.0203 5.70411 11.64002 z"; }
        }
    }
}