using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class WesternUnitedStatesModel : RegionModelBase
    {
        public WesternUnitedStatesModel(Region region) : base(region) {}

        public override string Name => Resources.WESTERN_UNITED_STATES;

        public override Point NamePosition => new Point(50, 180);

        public override string Path => "m 126.26907 176.05061 86.87312 0.25254 7.57614 4.54568 -8.83883 61.87185 -63.38708 27.77919 -59.851534 -22.47589 z";
    }
}