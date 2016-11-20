using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class SouthAfricaModel : RegionModelBase
    {
        public SouthAfricaModel(IRegion region) : base(region) {}

        public override string Name => Resources.SOUTH_AFRICA;

        public override Point NamePosition => new Point(690, 510);

        public override string Path => "m 673.77175 479.09637 26.76904 154.55334 32.82996 2.02031 47.47717 -65.15484 23.73858 -66.67007 -48.48732 -13.13198 -33.33503 15.15228 z";
    }
}