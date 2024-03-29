using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class EgyptModel : RegionModelBase
    {
        public EgyptModel(Region region) : base(region) {}

        public override string Name => Resources.EGYPT;

        public override Point NamePosition => new Point(700, 260);

        public override string Path => "m 666.1956 255.85266 105.56095 17.17259 4.54568 14.14214 -7.57614 5.05076 12.6269 24.74874 -53.033 19.69797 -64.64977 -29.7995 z";
    }
}