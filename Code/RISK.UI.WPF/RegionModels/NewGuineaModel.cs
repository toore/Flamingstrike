using System.Windows;
using RISK.Core;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class NewGuineaModel : RegionModelBase
    {
        public NewGuineaModel(IRegion region) : base(region) {}

        public override string Name => Resources.NEW_GUINEA;

        public override Point NamePosition => new Point(1240, 430);

        public override string Path => "m 1235.4166 444.24611 -14.6472 25.75889 97.4797 47.47717 11.6167 -47.47717 z";
    }
}