using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class BrazilModel : RegionModelBase
    {
        public BrazilModel(IRegion region) : base(region) {}

        public override string Name => Resources.BRAZIL;

        public override Point NamePosition => new Point(360, 450);

        public override string Path => "m 291.07143 458 87.5 -37.14286 76.78571 54.28572 L 388.92857 625.5 z";
    }
}