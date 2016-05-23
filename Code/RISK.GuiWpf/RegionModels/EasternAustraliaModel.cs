using System.Windows;
using GuiWpf.Properties;
using RISK.Core;

namespace GuiWpf.RegionModels
{
    public class EasternAustraliaModel : RegionModelBase
    {
        public EasternAustraliaModel(IRegion region) : base(region) {}

        public override string Name => Resources.EASTERN_AUSTRALIA;

        public override Point NamePosition => new Point(1240, 530);

        public override string Path => "m 1206.6272 526.06847 72.2259 -8.08123 29.7995 78.28683 -79.802 61.11423 z";
    }
}