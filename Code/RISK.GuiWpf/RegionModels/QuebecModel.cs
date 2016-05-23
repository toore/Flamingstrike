using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.RegionModels
{
    public class QuebecModel : RegionModelBase
    {
        public QuebecModel(IRegion region) : base(region) {}

        public override string Name => Resources.QUEBEC;

        public override Point NamePosition => new Point(320, 100);

        public override string Path => "m 306.5813 163.17116 41.16371 -57.83123 56.56855 50.00255 -92.6815 42.17387 z";
    }
}