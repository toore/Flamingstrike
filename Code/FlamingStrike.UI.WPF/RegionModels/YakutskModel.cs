using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class YakutskModel : RegionModelBase
    {
        public YakutskModel(Region region) : base(region) {}

        public override string Name => Resources.YAKUTSK;

        public override Point NamePosition => new Point(1060, 40);

        public override string Path => "M 981.86828 51.801845 1175.3125 71.49982 1125.815 131.6039 1011.6678 107.36023 z";
    }
}