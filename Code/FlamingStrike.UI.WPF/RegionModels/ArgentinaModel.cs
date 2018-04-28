using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class ArgentinaModel : RegionModelBase
    {
        public ArgentinaModel(Region region) : base(region) {}

        public override string Name => Resources.ARGENTINA;

        public override Point NamePosition => new Point(300, 590);

        public override string Path => "m 270 528 86.42857 41.78571 32.14286 56.07143 -38.92857 111.78572 -37.5 -37.14286 z";
    }
}