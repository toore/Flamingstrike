using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class SouthernEuropeModel : RegionModelBase
    {
        public SouthernEuropeModel(IRegion region) : base(region) {}

        public override string Name => Resources.SOUTHERN_EUROPE;

        public override Point NamePosition => new Point(640, 190);

        public override string Path => "m 653.06362 183.62675 -1.01015 20.70813 38.89088 26.76904 5.05076 -8.58629 -26.26397 -21.71828 12.12182 -6.06091 19.39716 17.65436 17.47342 31.8431 5.05076 -7.57614 -5.05076 -11.61675 21.71828 -20.70813 -23.73859 -17.17259 z";
    }
}