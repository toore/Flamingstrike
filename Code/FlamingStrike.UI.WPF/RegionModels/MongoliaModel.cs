using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class MongoliaModel : RegionModelBase
    {
        public MongoliaModel(Region region) : base(region) {}

        public override string Name => Resources.MONGOLIA;

        public override Point NamePosition => new Point(1100, 170);

        public override string Path => "m 1110.6627 214.4364 67.6802 41.92134 -5.5558 -75.76145 -184.35283 -16.66751 -9.09138 11.61675 69.70051 51.51778 z";
    }
}