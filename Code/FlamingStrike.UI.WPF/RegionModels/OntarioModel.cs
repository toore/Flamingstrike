using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class OntarioModel : RegionModelBase
    {
        public OntarioModel(Region region) : base(region) {}

        public override string Name => Resources.ONTARIO;

        public override Point NamePosition => new Point(240, 130);

        public override string Path => "m 244.70945 118.72445 30.55712 0.50508 30.30458 43.43656 5.55583 34.59772 -39.39595 19.1929 -58.3363 -41.16372 z";
    }
}