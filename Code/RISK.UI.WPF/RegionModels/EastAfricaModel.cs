using System.Windows;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class EastAfricaModel : RegionModelBase
    {
        public EastAfricaModel(IRegion region) : base(region) {}

        public override string Name => Resources.EAST_AFRICA;

        public override Point NamePosition => new Point(740, 370);

        public override string Path => "m 727.30983 337.16994 53.53809 -19.69798 40.91118 69.19545 32.32488 -6.56599 -51.01271 124.24877 -46.46701 -13.63706 -33.33504 -104.55079 z";
    }
}