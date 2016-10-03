using System.Windows;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class UralModel : RegionModelBase
    {
        public UralModel(IRegion region) : base(region) {}

        public override string Name => Resources.URAL;

        public override Point NamePosition => new Point(870, 80);

        public override string Path => "m 843.47738 74.025201 44.44671 -15.152288 78.7919 70.710677 5.05076 56.06347 -8.5863 4.54568 -97.47972 -25.25381 z";
    }
}