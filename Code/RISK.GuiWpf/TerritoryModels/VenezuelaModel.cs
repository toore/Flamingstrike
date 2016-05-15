using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public class VenezuelaModel : RegionModelBase
    {
        public VenezuelaModel(IRegion region) : base(region) {}

        public override string Name => Resources.VENEZUELA;

        public override Point NamePosition => new Point(270, 370);

        public override string Path => "m 250.77037 436.66996 6.56599 -32.82995 30.55712 -29.04189 90.66119 46.21448 -87.12566 37.62818 z";
    }
}