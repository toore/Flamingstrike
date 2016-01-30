using System.Windows;
using GuiWpf.Properties;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.TerritoryModels
{
    public class SouthernEuropeModel : RegionModelBase
    {
        public SouthernEuropeModel(IRegion region) : base(region) {}

        public override string Name
        {
            get { return Resources.SOUTHERN_EUROPE; }
        }

        public override Point NamePosition
        {
            get { return new Point(640, 190); }
        }

        public override string Path
        {
            get { return "m 653.06362 183.62675 -1.01015 20.70813 38.89088 26.76904 5.05076 -8.58629 -26.26397 -21.71828 12.12182 -6.06091 19.39716 17.65436 17.47342 31.8431 5.05076 -7.57614 -5.05076 -11.61675 21.71828 -20.70813 -23.73859 -17.17259 z"; }
        }
    }
}