using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public class AfghanistanModel : RegionModelBase
    {
        public AfghanistanModel(IRegion region) : base(region) {}

        public override string Name => Resources.AFGHANISTAN;
        public override Point NamePosition => new Point(830, 170);
        public override string Path => "m 802.06112 212.92118 64.14469 -48.48733 96.97464 26.26397 -72.73098 58.08377 z";
    }
}