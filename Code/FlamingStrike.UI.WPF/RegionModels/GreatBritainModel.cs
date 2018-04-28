using System.Windows;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.RegionModels
{
    public class GreatBritainModel : RegionModelBase
    {
        public GreatBritainModel(Region region) : base(region) {}

        public override string Name => Resources.GREAT_BRITAIN;

        public override Point NamePosition => new Point(525, 110);

        public override string Path => "m 584.37325 142.2105 -3.53554 21.2132 13.63706 0.50508 9.09138 -17.67767 z " +
                                       "m 25.25381 -16.66752 -3.03046 18.68782 -3.03045 25.75889 32.82995 -13.63706 -19.69797 -29.29442 z";
    }
}