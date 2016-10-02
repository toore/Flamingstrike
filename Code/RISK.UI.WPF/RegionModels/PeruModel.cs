using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class PeruModel : RegionModelBase
    {
        public PeruModel(IRegion region) : base(region) {}

        public override string Name => Resources.PERU;

        public override Point NamePosition => new Point(260, 460);

        public override string Path => "M 251.07143 437.28571 242.14286 478.35714 270 527.28571 357.14286 570.14286 291.42857 458.71429 z";
    }
}