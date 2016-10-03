using System.Windows;
using RISK.GameEngine;
using RISK.UI.WPF.Properties;

namespace RISK.UI.WPF.RegionModels
{
    public class CongoModel : RegionModelBase
    {
        public CongoModel(IRegion region) : base(region) {}

        public override string Name => Resources.CONGO;

        public override Point NamePosition => new Point(680, 420);

        public override string Path => "m 664.1753 433.6395 58.58885 -48.48731 33.84011 104.55078 -34.34519 16.16244 -47.98225 -26.76903 z";
    }
}