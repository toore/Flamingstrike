using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class SiberiaModel : RegionModelBase
    {
        public SiberiaModel(Region region) : base(region) {}

        public override string Name => Resources.SIBERIA;

        public override Point NamePosition => new Point(930, 40);

        public override string Path => "m 887.92409 59.377989 60.60915 -25.253814 34.34519 18.687823 36.87057 71.720832 -40.40611 51.0127 -8.58629 2.52538 -3.53554 -48.48732 z";
    }
}