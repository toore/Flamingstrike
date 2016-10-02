using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class NorthwestRegionModel : RegionModelBase
    {
        public NorthwestRegionModel(IRegion region) : base(region) {}

        public override string Name => Resources.NORTHWEST_TERRITORY;

        public override Point NamePosition => new Point(157.07872, 40);

        public override string Path => "m 157.07872 72.76251 97.22718 7.323606 67.68022 -16.667517 33.58758 11.869292 -80.05459 43.689089 -163.64472 -2.0203 z" +
                                       "M 223.74879 59.377989 252.53814 45.993467 291.68155 64.68129 250.01276 75.540429 z";
    }
}