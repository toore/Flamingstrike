using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class AlbertaModel : RegionModelBase
    {
        public AlbertaModel(Region alberta) : base(alberta) {}

        public override string Name => Resources.ALBERTA;
        public override Point NamePosition => new Point(140, 110);
        public override string Path => "m 125.76399 175.79807 87.12566 0.25254 31.8198 -57.07362 -132.07745 -2.0203 5.70411 11.64002 z";
    }
}