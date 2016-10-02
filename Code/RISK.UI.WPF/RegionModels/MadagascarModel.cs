using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class MadagascarModel : RegionModelBase
    {
        public MadagascarModel(IRegion region) : base(region) {}

        public override string Name => Resources.MADAGASCAR;

        public override Point NamePosition => new Point(830, 520);

        public override string Path => "m 843.47738 514.45171 -23.23351 25.75889 -6.06092 42.93148 24.74874 3.03046 z";
    }
}