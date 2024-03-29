using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class KamchatkaModel : RegionModelBase
    {
        public KamchatkaModel(Region region) : base(region) {}

        public override string Name => Resources.KAMCHATKA;

        public override Point NamePosition => new Point(1190, 60);

        public override string Path => "m 1174.8074 71.49982 108.0863 13.131983 -33.335 77.276667 -47.9822 -50.50762 -50.0026 31.31472 60.1041 43.94164 -36.3655 21.71828 -2.5254 -27.7792 -18.6878 -2.0203 -28.2843 -46.97209 z";
    }
}