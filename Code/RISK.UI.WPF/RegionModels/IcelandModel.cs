using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class IcelandModel : RegionModelBase
    {
        public IcelandModel(IRegion region) : base(region) {}

        public override string Name => Resources.ICELAND;

        public override Point NamePosition => new Point(560, 40);

        public override string Path => "M 546.9976 90.692718 552.55344 107.36023 585.3834 94.733328 575.28187 82.106421 z";
    }
}