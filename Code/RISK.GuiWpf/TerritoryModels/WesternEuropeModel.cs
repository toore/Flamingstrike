using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.TerritoryModels
{
    public class WesternEuropeModel : RegionModelBase
    {
        public WesternEuropeModel(IRegion region) : base(region) {}

        public override string Name => Resources.WESTERN_EUROPE;

        public override Point NamePosition => new Point(500, 180);

        public override string Path => "m 636.3961 164.93893 -29.7995 14.14214 7.07107 26.76904 -24.74874 0 -8.08122 36.36549 33.84011 -2.02031 37.37565 -35.35533 0.50507 -22.22336 z";
    }
}