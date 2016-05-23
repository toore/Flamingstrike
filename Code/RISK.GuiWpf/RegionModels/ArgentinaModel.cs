using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.RegionModels
{
    public class ArgentinaModel : RegionModelBase
    {
        public ArgentinaModel(IRegion region) : base(region) {}

        public override string Name => Resources.ARGENTINA;

        public override Point NamePosition => new Point(300, 590);

        public override string Path => "m 270 528 86.42857 41.78571 32.14286 56.07143 -38.92857 111.78572 -37.5 -37.14286 z";
    }
}