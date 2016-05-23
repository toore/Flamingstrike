using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public class WesternUnitedStatesModel : RegionModelBase
    {
        public WesternUnitedStatesModel(IRegion region) : base(region) {}

        public override string Name => Resources.WESTERN_UNITED_STATES;

        public override Point NamePosition => new Point(50, 180);

        public override string Path => "m 126.26907 176.05061 86.87312 0.25254 7.57614 4.54568 -8.83883 61.87185 -63.38708 27.77919 -59.851534 -22.47589 z";
    }
}