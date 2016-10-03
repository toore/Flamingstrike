using System.Windows;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class NorthAfricaModel : RegionModelBase
    {
        public NorthAfricaModel(IRegion region) : base(region) {}

        public override string Name => Resources.NORTH_AFRICA;

        public override Point NamePosition => new Point(570, 290);

        public override string Path => "m 666.70068 240.70037 -3.53553 66.16499 64.14468 29.29443 -5.05076 48.9924 -58.08377 48.48732 -123.23861 -53.03301 34.34518 -123.74369 z";
    }
}