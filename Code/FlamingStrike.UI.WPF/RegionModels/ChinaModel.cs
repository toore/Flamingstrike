using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class ChinaModel : RegionModelBase
    {
        public ChinaModel(Region region) : base(region) {}

        public override string Name => Resources.CHINA;

        public override Point NamePosition => new Point(1050, 230);

        public override string Path => "m 925.80481 219.99224 180.81729 106.57109 45.4569 -11.11167 8.0812 -40.91118 -47.9822 -60.10408 -63.4951 12.04032 -69.84508 -50.42611 -7.07107 2.52538 0 7.57614 -9.09137 3.53554 z";
    }
}