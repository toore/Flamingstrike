using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class IrkutskModel : RegionModelBase
    {
        public IrkutskModel(Region region) : base(region) {}

        public override string Name => Resources.IRKUTSK;

        public override Point NamePosition => new Point(1030, 100);

        public override string Path => "m 1012.1729 108.37039 113.6421 23.73858 27.7792 45.96194 -165.66501 -14.64721 31.81981 -40.4061 z";
    }
}