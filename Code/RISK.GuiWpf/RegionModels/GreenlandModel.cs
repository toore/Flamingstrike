using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public class GreenlandModel : RegionModelBase
    {
        public GreenlandModel(IRegion region) : base(region) {}

        public override string Name => Resources.GREENLAND;

        public override Point NamePosition => new Point(450, 20);

        public override string Path => "m 418.20315 33.6191 42.42641 82.83251 104.04571 -65.659917 18.18275 -38.385797 z";
    }
}