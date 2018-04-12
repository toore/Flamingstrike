using System.Windows;
using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class SiamModel : RegionModelBase
    {
        public SiamModel(Region region) : base(region) {}

        public override string Name => Resources.SIAM;

        public override Point NamePosition => new Point(1060, 320);

        public override string Path => "m 1052.0738 293.73338 55.0534 33.33503 l 13.132 56.06347 -17.6777 14.64721 -24.2437 -24.24366 23.2336 63.13453 -19.1929 -11.11167 -50.0026 -102.53049 z";
    }
}