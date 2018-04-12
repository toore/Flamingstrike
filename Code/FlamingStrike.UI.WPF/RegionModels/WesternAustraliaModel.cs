using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class WesternAustraliaModel : RegionModelBase
    {
        public WesternAustraliaModel(Region region) : base(region) {}

        public override string Name => Resources.WESTERN_AUSTRALIA;

        public override Point NamePosition => new Point(1080, 560);

        public override string Path => "m 1207.6374 526.06847 -74.7513 40.91117 -10.1015 68.69038 106.571 20.70812 z";
    }
}