using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class IndiaModel : RegionModelBase
    {
        public IndiaModel(Region region) : base(region) {}

        public override string Name => Resources.INDIA;

        public override Point NamePosition => new Point(940, 270);

        public override string Path => "m 889.94439 249.28667 36.36549 -28.78935 126.26912 73.74114 -81.31732 110.6117 -75.76145 -98.99495 z";
    }
}