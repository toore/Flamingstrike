using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class NorthernEuropeModel : RegionModelBase
    {
        public NorthernEuropeModel(Region region) : base(region) {}

        public override string Name => Resources.NORTHERN_EUROPE;

        public override Point NamePosition => new Point(630, 120);

        public override string Path => "m 635.89103 163.92878 23.73858 -20.70813 48.9924 0 7.07107 43.94164 -62.62946 -3.53553 z";
    }
}